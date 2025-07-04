using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using News.Repositories.Interfaces;
using News.Repositories.Models;
using AutoMapper;
using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data.SQLite;
using System.Data.Entity;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Globalization;


namespace News.Repositories
{

    public class NewsItemsRepository : INewsItemsRepository
    {

        private readonly ILogger<NewsItemsRepository> _logger;

        private string _connectionString = "";

        public NewsItemsRepository(IConfiguration configuration, ILogger<NewsItemsRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("NewsDbSqlServer") ?? throw new Exception("Connection Configuration Error");
            _logger = logger;
        }

        public NewsItem Get(int id)
        {
            try
            {
                _logger.LogDebug("Getting NewsItems(Id) from Database.");

                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                string stm = "SELECT * FROM newsitems WHERE id = @Id";

                using var cmd = new SQLiteCommand(stm, con);
                cmd.Parameters.AddWithValue("@Id", id);

                using SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var news = new NewsItem
                    {
                        Id = rdr.GetInt32(0),
                        Guid = rdr.GetString(1),
                        SourceId = rdr.GetInt32(2),
                        Title = rdr.GetString(3),
                        Description = rdr.GetString(4),
                        Content = rdr.GetString(5),
                        HtmlContent = rdr.GetString(6),
                        Headline = rdr.GetString(7),
                        LinkUrl = rdr.GetString(8),
                        ImageUrl = rdr.GetString(9),
                        ThumbUrl = rdr.GetString(10),
                        LanguageId = rdr.GetInt32(11),
                        PublishedAt = rdr.GetDateTime(12),
                        CreatedAt = rdr.GetDateTime(13),
                        //UpdatedAt = rdr.GetDateTime(14),
                        //DeletedAt = rdr.GetDateTime(15),
                    };

                    return news;
                }
                return null;
            }

            catch (Exception ex)
            {
                _logger.LogDebug(ex + " There was a problem getting all the newsitems from the database, returning empty newsitems.");
                return null;
            }
        }

        public IEnumerable<NewsItem> Get()
        {
            try
            {
                _logger.LogDebug("Getting NewsItems from Database.");

                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                string stm = "SELECT * FROM newsitems";

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                var List = new List<NewsItem>();
                while (rdr.Read())
                {
                    var news = new NewsItem
                    {
                        Id = rdr.GetInt32(0),
                        Guid = rdr.GetString(1),
                        SourceId = rdr.GetInt32(2),
                        Title = rdr.GetString(3),
                        Description = rdr.GetString(4),
                        Content = rdr.GetString(5),
                        HtmlContent = rdr.GetString(6),
                        Headline = rdr.GetString(7),
                        LinkUrl = rdr.GetString(8),
                        ImageUrl = rdr.GetString(9),
                        ThumbUrl = rdr.GetString(10),
                        LanguageId = rdr.GetInt32(11),
                        PublishedAt = rdr.GetDateTime(12),
                        CreatedAt = rdr.GetDateTime(13),
                        //UpdatedAt = rdr.GetDateTime(14),
                        //DeletedAt = rdr.GetDateTime(15),
                    };

                    List.Add(news);
                }
                return List;
            }

            catch (Exception ex)
            {
                _logger.LogDebug(ex + " There was a problem getting all the newsitems from the database, returning empty newsitems.");
                return Enumerable.Empty<NewsItem>();
            }

            return null;
        }

        public NewsItem Create(NewsItem newsItem)
        {
            _logger.LogDebug("NewsItems Repository Create called.");

            try
            {
                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "INSERT INTO newsitems (guid, sourceId, title, description, content, htmlContent, headline, linkUrl, imageUrl, thumbUrl, languageId, publishedAt, createdAt, updatedAt, deletedAt) VALUES (@Guid, @SourceId, @Title, @Description, @Content, @HtmlContent, @Headline, @LinkUrl, @ImageUrl, @ThumbUrl, @LanguageId, @PublishedAt, @CreatedAt, @UpdatedAt, @DeletedAt) ON CONFLICT DO NOTHING";

                cmd.Parameters.AddWithValue("@Guid", newsItem.Guid);
                cmd.Parameters.AddWithValue("@SourceId", newsItem.SourceId);
                cmd.Parameters.AddWithValue("@Title", newsItem.Title);
                cmd.Parameters.AddWithValue("@Description", newsItem.Description);
                cmd.Parameters.AddWithValue("@Content", newsItem.Content);
                cmd.Parameters.AddWithValue("@HtmlContent", newsItem.HtmlContent);
                cmd.Parameters.AddWithValue("@Headline", newsItem.Headline);
                cmd.Parameters.AddWithValue("@LinkUrl", newsItem.LinkUrl);
                cmd.Parameters.AddWithValue("@ThumbUrl", newsItem.ThumbUrl);
                cmd.Parameters.AddWithValue("@LanguageId", newsItem.LanguageId);
                cmd.Parameters.AddWithValue("@ImageUrl", newsItem.ImageUrl);
                cmd.Parameters.AddWithValue("@CreatedAt", newsItem.CreatedAt);
                cmd.Parameters.AddWithValue("@PublishedAt", newsItem.PublishedAt);
                cmd.Parameters.AddWithValue("@UpdatedAt", newsItem.UpdatedAt);
                cmd.Parameters.AddWithValue("@DeletedAt", newsItem.DeletedAt);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                _logger.LogDebug($"{ex}");
            }
            return null;
        }

        public NewsItem Update(NewsItem newsItem)
        {
            _logger.LogDebug("NewsItem Repository Update Called.");
            try
            {
                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "UPDATE newsitems SET sent = @bool WHERE title = @title";

                cmd.Parameters.AddWithValue("@bool", true);
                cmd.Parameters.AddWithValue("@title", newsItem.Title);


                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                _logger.LogDebug($"{ex}");
            }

            return null;
        }

        public void Delete(NewsItem newsItem)
        {
            using var con = new SqlConnection(_connectionString);
            var SqlAll = "DELETE FROM newsitems WHERE id = @Id";
            var parameter = new { id = newsItem.Id };
            var Alles = con.Query(SqlAll, parameter);
        }

        public List<NewsItem> NotSent()
        {
            try
            {
                _logger.LogDebug("Getting NewsItems (Not Sent) from Database.");

                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                string stm = "SELECT * FROM newsitems WHERE sent = 0";

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                var List = new List<NewsItem>();
                while (rdr.Read())
                {
                    var news = new NewsItem
                    {
                        Id = rdr.GetInt32(0),
                        Guid = rdr.GetString(1),
                        SourceId = rdr.GetInt32(2),
                        Title = rdr.GetString(3),
                        Description = rdr.GetString(4),
                        Content = rdr.GetString(5),
                        HtmlContent = rdr.GetString(6),
                        Headline = rdr.GetString(7),
                        LinkUrl = rdr.GetString(8),
                        ImageUrl = rdr.GetString(9),
                        ThumbUrl = rdr.GetString(10),
                        LanguageId = rdr.GetInt32(11),
                        PublishedAt = rdr.GetDateTime(12),
                        CreatedAt = rdr.GetDateTime(13),
                        //UpdatedAt = rdr.GetDateTime(14),
                        //DeletedAt = rdr.GetDateTime(15),
                    };

                    List.Add(news);
                }
                if (List == null)
                {

                    return null;
                }
                return List;
            }

            catch (Exception ex)
            {
                _logger.LogDebug(ex + " There was a problem getting all the newsitems from the database, returning empty newsitems.");
            }
            return null;
        }
    } 
}