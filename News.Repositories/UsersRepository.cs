using Dapper;
//using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using News.Repositories.Interfaces;
using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Globalization;
using System.CodeDom;
using System.Reflection.PortableExecutable;
using static System.Data.Entity.Infrastructure.Design.Executor;
using Microsoft.Identity.Client;

namespace News.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly ILogger<User> _logger;
        private string _connectionString = "";

        public UsersRepository(IConfiguration configuration, ILogger<User> logger)
        {
            _connectionString = configuration.GetConnectionString("NewsDbSqlServer") ?? throw new Exception("Connection Configuration Error");
            _logger = logger;
        }

        public User Login(string mail, string password)
        {
            _logger.LogDebug("User Repository Read Called.");

            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            string stm = "SELECT * FROM users WHERE mail = @mail AND password = @password";

            using var cmd = new SQLiteCommand(stm, con);
            cmd.Parameters.AddWithValue("@mail", mail);
            cmd.Parameters.AddWithValue("@password", password);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var user = new User
                {
                    Id = rdr.GetInt32(0),
                    Mail = rdr.GetString(1),
                    Password = rdr.GetString(2),
                    //SourceId = rdr.GetInt32(3),
                    Firstname = rdr.GetString(4),
                    Lastname = rdr.GetString(5),
                    //Telephone_number = rdr.GetInt32(6),
                    //CreatedAt = DateTime.ParseExact(rdr.GetString(8), "dd-MM-yyyy", null),
                };

                if (!rdr.IsDBNull(9))
                {
                    user.UpdatedAt = DateTime.ParseExact(rdr.GetString(9), "yyyy-MM-dd HH:mm:ss", null);
                }

                else
                {
                    user.UpdatedAt = null;
                }

                return user;
            }
            return null;
        }

        public User Add(string mail, string password, string firstName, string lastName)
        {
            _logger.LogDebug("Users Repository Add Called.");
            try
            {
                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "INSERT INTO users (mail, password, firstName, lastName) VALUES (@mail, @password, @lname, @fname)";

                cmd.Parameters.AddWithValue("@mail", mail);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@fname", firstName);
                cmd.Parameters.AddWithValue("@lname", lastName);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                _logger.LogDebug($"{ex}");
            }

            return null;
        }

        public User Update(string mail, string password, int sourceid, int telephonenumber)
        {
            _logger.LogDebug("Users Repository Update Called.");
            try
            {
                using var con = new SQLiteConnection(_connectionString);
                con.Open();

                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "UPDATE users SET sourceid = @sourceid, telephoneNumber = @telephonenumber WHERE mail = @mail AND password = @password ";

                cmd.Parameters.AddWithValue("@mail", mail);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@sourceid", sourceid);
                cmd.Parameters.AddWithValue("@telephonenumber", telephonenumber);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                _logger.LogDebug($"{ex}");
            }

            return null;
        }

        public List<User> Get() 
        {

            _logger.LogDebug("User Repository Read Called.");

            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            string stm = "SELECT * FROM users WHERE sourceid > 0";

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            var list = new List<User>();

            while (rdr.Read())
            {
                var user = new User
                {
                    Id = rdr.GetInt32(0),
                    Mail = rdr.GetString(1),
                    Password = rdr.GetString(2),
                    SourceId = rdr.GetInt32(3),
                    Firstname = rdr.GetString(4),
                    Lastname = rdr.GetString(5),
                    Telephone_number = rdr.GetInt32(6),
                    //CreatedAt = DateTime.ParseExact(rdr.GetString(8), "dd-MM-yyyy", null),
                };

                list.Add(user);

                //if (!rdr.IsDBNull(9))
                //{
                //    user.UpdatedAt = DateTime.ParseExact(rdr.GetString(9), "yyyy-MM-dd HH:mm:ss", null);
                //}

                //else
                //{
                //    user.UpdatedAt = null;
                //}
            }
            return list;
        }
    }
}
