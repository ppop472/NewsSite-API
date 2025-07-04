using Microsoft.Extensions.Configuration;
using News.Business.Interfaces;
using News.Business.Models;
using News.Repositories.Models;
using News.Repositories;
using AutoMapper;
using News.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;

namespace News.Business
{
    public class NewsItemService : INewsItemsService
    {

        private readonly INewsItemsRepository _newsItemsRepository;

        private readonly ILanguageRepository _languageRepository;

        private readonly ISourceRepository _sourceRepository;

        private readonly ILogger<NewsItemService> _logger;

        private readonly IMailerService _mailerService;

        //private string _connectionString = "";
        public NewsItemService(INewsItemsRepository newsItemsRepository, ILanguageRepository languageRepository, ISourceRepository sourceRepository, ILogger<NewsItemService> logger, IMailerService mailer)
        {
            _newsItemsRepository = newsItemsRepository;
            _languageRepository = languageRepository;   
            _sourceRepository = sourceRepository;
            _logger = logger;
            _mailerService = mailer;
        }


        public NewsItemDto ToNewsItemDto(NewsItem newsItem, Languages languages, Sources sources)
        {
            if(sources == null || languages == null || newsItem == null)
            {
                _logger.LogDebug("Source or languages was null");
                return new NewsItemDto();
            }

            try
            {
                _logger.LogDebug("Converting Newsitem to NewsItemDto.");
                var newsItemDto = new NewsItemDto
                {
                    Id = newsItem.Id,
                    Source = sources.Name,
                    Title = newsItem.Title,
                    Description = newsItem.Description,
                    Content = newsItem.Content,
                    HtmlContent = newsItem.HtmlContent,
                    Headline = newsItem.Headline,
                    LinkUrl = newsItem.LinkUrl,
                    ImageUrl = newsItem.ImageUrl,
                    ThumbUrl = newsItem.ThumbUrl,
                    Language = languages.Code,
                    PublishedAt = newsItem.PublishedAt    
                };

                return newsItemDto;
            }
            
            catch(Exception ex)
            {
                _logger.LogError(ex + "There was a problem when trying to convert NewsItem to NewsItemDto, or its empty.");
                return new NewsItemDto();
            }
            return null;
        }
        public NewsItem ToNewsItem(NewsItemDto newsItemDto)
        {
            var NewsItem = new NewsItem
            {
                Id = newsItemDto.Id,
                Guid = "",
                SourceId = 1,
                Title = "Title",
                Description = newsItemDto.Description,
                Content = newsItemDto.Content,
                HtmlContent = newsItemDto.HtmlContent,
                Headline = newsItemDto.Headline,
                LinkUrl = newsItemDto.LinkUrl,
                ImageUrl = newsItemDto.ImageUrl,
                ThumbUrl = newsItemDto.ThumbUrl,
                LanguageId = 1,
                PublishedAt = newsItemDto.PublishedAt,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                DeletedAt = null,
            };

            return NewsItem;
        }

        // New News Added function 

        // Send to mail/team/idk new News of the good sourceid

        public NewsItemDto Get(int id)
        {
            _logger.LogDebug("Calling LanguageRespository Get.");
            var newsItem = _newsItemsRepository.Get(id);

            _logger.LogDebug("Calling SourceRespository Get.");
            var language = _languageRepository.Get();

            _logger.LogDebug("Calling NewsItemRespository Get.");
            var source = _sourceRepository.Get();

            if (newsItem == null || language == null || source == null)
            {
                _logger.LogDebug("Newsitem, language or source was empty. Returning empty NewsItemDto");
                return new NewsItemDto();
            }

            _logger.LogDebug("Searching Language with the correct ID");
            var languages = language.FirstOrDefault(l => l.Id == newsItem.LanguageId);

            _logger.LogDebug("Searching Source with the correct ID");
            var sources = source.FirstOrDefault(s => s.Id == newsItem.SourceId);

            _logger.LogDebug("Calling NewsItem to NewsItemDto converter.");
            var newsitemdto = ToNewsItemDto(newsItem, languages, sources);

            return newsitemdto;
        }

        public IEnumerable<NewsItemDto> Get()
        {
            _logger.LogDebug("Calling LanguageRespository Get.");
            var language = _languageRepository.Get();

            _logger.LogDebug("Calling SourceRespository Get.");
            var source = _sourceRepository.Get();

            _logger.LogDebug("Calling NewsItemRespository Get.");
            var newsItem = _newsItemsRepository.Get();
            
            var newsItemDtos = newsItem.Select(newsItem =>
            {
                _logger.LogDebug("Searching Language with the correct ID");
                var languages = language.FirstOrDefault(l => l.Id == newsItem.LanguageId);

                _logger.LogDebug("Searching Source with the correct ID");
                var sources = source.FirstOrDefault(s => s.Id == newsItem.SourceId);

                _logger.LogDebug("Calling NewsItem to NewsItemDto converter.");
                var newsitemdto = ToNewsItemDto(newsItem, languages, sources);

                return newsitemdto;
            });

            return newsItemDtos;    
        }

        public NewsItemDto Create(NewsItemDto newsItemDto)
        {
            _logger.LogDebug("Create is called.");

            _logger.LogDebug("Calling NewsItem to NewsItemDto converter.");
            var newsitem = ToNewsItem(newsItemDto);

            _logger.LogDebug("Calling LanguageRespository Get.");
            var language = _languageRepository.Get();

            _logger.LogDebug("Calling SourceRespository Get.");
            var source = _sourceRepository.Get();

            _logger.LogDebug("Calling NewsItemRepository Create.");
            var createdNewsItem = _newsItemsRepository.Create(newsitem);

            _logger.LogDebug("Calling MailerService.");
            var mailerservice = _mailerService.Send();

            return null;
        }

        public NewsItemDto Update(NewsItemDto newsItemDto)
        {
            var newsitem = ToNewsItem(newsItemDto);

            var updatedNewsItem = _newsItemsRepository.Update(newsitem);

            return null;
        }

        public void Delete(NewsItemDto newsItemDto)
        {
            var newsItem = ToNewsItem(newsItemDto);

            _newsItemsRepository.Delete(newsItem);
        }
    }
}