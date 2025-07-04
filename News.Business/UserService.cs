using Microsoft.Extensions.Logging;
using News.Business.Interfaces;
using News.Business.Models;
using News.Repositories.Interfaces;
using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace News.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<User> _logger;

        public UserService(IUserRepository userRepository, ILogger<User> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public User Login(string mail, string password)
        {

            _logger.LogDebug("Calling User.Respository Read.");
            var users = _userRepository.Login(mail, password);

            return users;
        }

        public User Add(string mail, string password, string firstName, string lastName)
        {

            _logger.LogDebug("Calling User.Respository Read.");
            var users = _userRepository.Add(mail, password, firstName, lastName);

            return users;
        }

        public User Update(string mail, string password, int sourceid, int telephonenumber)
        {

            _logger.LogDebug("Calling User.Respository Read.");
            var users = _userRepository.Update(mail, password, sourceid, telephonenumber);

            return users;
        }
        public List<User> Get()
        {
            _logger.LogDebug("Calling User.Respository Get.");
            var users = _userRepository.Get();

            return users;
        }
    }
}
