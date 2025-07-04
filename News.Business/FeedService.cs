using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using News.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Security.Cryptography;
using Azure;
using System;
using News.Repositories.Models;
using System.Text.RegularExpressions;
using System.Linq;
using News.Business.Interfaces;

namespace News.Business
{
    public class FeedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly INewsItemsRepository _newsItemsRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ISourceRepository _sourceRepository;
        private readonly IMailerService _mailerService;

        private readonly IServiceScope _serviceScope;

        public FeedService(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
            _logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<FeedService>>();
            _newsItemsRepository = _serviceScope.ServiceProvider.GetRequiredService<INewsItemsRepository>();
            _languageRepository = _serviceScope.ServiceProvider.GetRequiredService<ILanguageRepository>();
            _sourceRepository = _serviceScope.ServiceProvider.GetRequiredService<ISourceRepository>();
            _mailerService = _serviceScope.ServiceProvider.GetRequiredService<IMailerService>();
        }

        public override void Dispose()
        {
            _serviceScope.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int intervalTime = 5 * 60 * 1000; // = 5 minutes

            // Process as long as the program is not terminated
            while (!stoppingToken.IsCancellationRequested)
            {
                // Execute the processing of news feeds.
                await ReadNewsItemsAsync();

                // Wait 5 minutes
                await Task.Delay(intervalTime, stoppingToken);
            }
        }

        private async Task ReadNewsItemsAsync()
        {
            _logger.LogDebug("Reading News Sites began.");

            _logger.LogDebug("Calling SourceRepository Get.");
            var sources = _sourceRepository.Get();

            _logger.LogDebug("Calling LanguageRepository Get.");
            var languages = _languageRepository.Get();

            var urls = sources.Select(sources => sources.Url).ToList<string>();

            var sourceids = sources.Select(sources => sources.Id).ToList<int>();

            var languageids = languages.Select(languages => languages.Id).ToList<int>();




            var zipped = urls.Zip(sourceids, (first, second) => new { first, second })
                              .Zip(languageids, (pair, third) => (pair.first, pair.second, third))
                              .ToList();


            foreach (var item in zipped)
            {

                Console.WriteLine($"{item.first}, {item.second}, {item.third}");

                await ReadNewsItemsAsync(item.first, item.second, item.third);

            }
        }

        private async Task ReadNewsItemsAsync(string url, int sourceid, int languageids)
        {
            _logger.LogDebug("Reading xml");
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);

            var sz = new XmlSerializer(typeof(Rss));

            using var stream = response.Content.ReadAsStream();

            var rss = sz.Deserialize(stream) as Rss;

            var newsItem = new NewsItem();

            _logger.LogDebug("Filling Channels");
            foreach (var channel in rss.Channels)
            {
                foreach (var rssItem in channel.Items)
                {
                    var content = rssItem.Content;
                    var description = rssItem.Description;
                    var htmlcontent = rssItem.Content;

                    if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(description))
                    {
                        htmlcontent = content;
                        content = Regex
                            .Replace(content, @"<[^>]+>", "", RegexOptions.Singleline)
                            .Replace(@"\s+", " ");
                        description = rssItem.Description;
                    }

                    else if (!string.IsNullOrEmpty(content))
                    {
                        htmlcontent = content;
                        content = Regex
                            .Replace(content, @"<[^>]+>", "", RegexOptions.Singleline)
                            .Replace(@"\s+", " ");
                        description = "";
                    }

                    else
                    {
                        htmlcontent = description;
                        content = rssItem.Description;
                        content = Regex
                            .Replace(content, @"<[^>]+>", "", RegexOptions.Singleline)
                            .Replace(@"\s+", " ");
                        description = "";
                    }

                    //var pattern = (@"^(<p>).+$");
                    var pattern = (@"(?i)[>]$");

                    bool regexbool = Regex.IsMatch(htmlcontent, pattern);
                    if (!regexbool)
                    {
                        string htmltag = "<p>";
                        string htmlendtag = "</p>";

                        htmlcontent = htmltag + htmlcontent + htmlendtag;
                    }

                    newsItem.LanguageId = languageids;
                    newsItem.SourceId = sourceid;
                    newsItem.Title = rssItem.Title;
                    newsItem.Description = description;
                    newsItem.Content = content;
                    newsItem.HtmlContent = htmlcontent;
                    newsItem.ThumbUrl = "";


                    if (rssItem.Enclosures != null)
                    {
                        var hasImages = rssItem.Enclosures
                            .Any(enclosure => enclosure.Type == "image/jpeg");

                        var hasLinkUrl = rssItem.Content
                            .Any();

                        if (hasImages)
                        {
                            newsItem.ImageUrl = rssItem.Enclosures.First(enclosure => enclosure.Type == "image/jpeg").Url;
                        }
                    }

                    else
                    {
                        var reg1 = new Regex("src=(?:\"|\')?(?<imgSrc>[^>]*[^/].(?:jpg|bmp|gif|png))(?:\"|\')?");
                        var match1 = reg1.Match(rssItem.Content);
                        if (match1.Groups["imgSrc"].Success)
                        {
                            newsItem.ImageUrl = match1.Groups["imgSrc"].Value;
                        }
                    }

                    newsItem.LinkUrl = rssItem.Link;
                    newsItem.PublishedAt = DateTime.Parse(rssItem.PublishDate);
                    newsItem.Guid = "";
                    newsItem.CreatedAt = DateTime.Now;

                    // Insert into database
                    _logger.LogDebug("Calling NewsItemRepository Create.");
                    var sql = _newsItemsRepository.Create(newsItem);


                }
            }
            _logger.LogDebug("Calling Miailer Send from Feeder.");
            var mailer = _mailerService.Send();
        }
    }
}

