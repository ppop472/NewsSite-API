using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using News.Repositories.Interfaces;
using News.Repositories.Models;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using System.Reflection;

namespace News.Repositories
{
    public class SourcesRepository : ISourceRepository
    {
        private readonly ILogger<SourcesRepository> _logger;
        private string _connectionString = "";
        public SourcesRepository(IConfiguration configuration, ILogger<SourcesRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("NewsDbSqlServer") ?? throw new Exception("Connection Configuration Error");
            _logger = logger;

        }
        public IEnumerable<Sources> Get()
        {
            try
            {
                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                string stm = "SELECT * FROM source";

                using var cmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                var List = new List<Sources>();
                while (rdr.Read())
                {
                    var sources = new Sources
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1),
                        Url = rdr.GetString(3),
                        //Module = rdr.GetString(4),
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
                _logger.LogDebug(ex + " There was a problem getting all the sources from the database, returning empty sources.");
                return Enumerable.Empty<Sources>();
            }
            return null;
        }
    }
}
