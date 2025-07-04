using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;
using News.Repositories.Interfaces;
using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Repositories
{
    public class LanguagesRepository : ILanguageRepository
    {
        private readonly ILogger<LanguagesRepository> _logger;
        private string _connectionString = "";
        public LanguagesRepository(IConfiguration configuration, ILogger<LanguagesRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("NewsDbSqlServer") ?? throw new Exception("Connection Configuration Error");
            _logger = logger;

        }
        public IEnumerable<Languages> Get()
        {         
            try
            {
                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                string stm = "SELECT * FROM languages";

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                var List = new List<Languages>();
                while (rdr.Read())
                {
                    var sources = new Languages
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1),
                        Short = rdr.GetString(2),
                        Code = rdr.GetString(3),
                        CreatedAt = rdr.GetDateTime(4),                        
                        //CreatedAt = DateTime.ParseExact(rdr.GetString(8), "yyyy-MM-ddTHH:mm:ss", null),
                        //UpdatedAt = DateTime.ParseExact(rdr.GetString(6), "yyyy-MM-ddTHH:mm:ss", null),
                        //DeletedAt = DateTime.ParseExact(rdr.GetString(7), "yyyy-MM-ddTHH:mm:ss", null),
                    };

                    List.Add(sources);
                }
                return List;
            }

            catch (Exception ex)             
            {
                _logger.LogDebug(ex + " There was a problem getting all the languages from the database, returning empty languages.");
                return Enumerable.Empty<Languages>();
            }
            return null;
        }
    }
}
