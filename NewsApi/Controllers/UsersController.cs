using Microsoft.AspNetCore.Mvc;
using News.Business.Interfaces;
using News.Business.Models;
using Serilog.AspNetCore;
using Serilog.Configuration;
using Serilog.Settings.Configuration;
using Serilog.Sinks.File;
using Serilog.Sinks.Async;
using News.Business;
using News.Repositories.Interfaces;
using News.Repositories.Models;
using System.Net.Mail;
using FluentEmail.Core;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ILogger<NewsItemController> _logger;
        private readonly IMailerService _mailer;
        public UsersController(ILogger<NewsItemController> logger, IUserService userService, IMailerService mailer)
        {
            _logger = logger;
            _userService = userService;
            _mailer = mailer;
        }

        [HttpPost("Get")]
        public ActionResult LoginUser([FromBody] User user)
        {
            _logger.LogDebug("Api AllNews Called.");

            _logger.LogDebug("Calling Users.Service Read");

            var User = _userService.Login(user.Mail, user.Password);

            return Ok(User);
        }
        
        [HttpPost]

        public ActionResult AddUser(User user)
        {
            _logger.LogDebug("Api AddNews Called.");

            _logger.LogDebug("Calling Users.Service Add");

            var User = _userService.Add(user.Mail, user.Password, user.Firstname, user.Lastname);

            return Ok(User);
        }

        [HttpPut]

        public ActionResult Update(User user)
        {
            _logger.LogDebug("Api Update Called.");

            _logger.LogDebug("Calling Users.Service Update");

            var User = _userService.Update(user.Mail, user.Password, user.SourceId, user.Telephone_number);

            return Ok(User);
        }

        [HttpGet("All")]

        public ActionResult Get()
        {
            _logger.LogDebug("Api Get Called.");

            _logger.LogDebug("Calling Users.Service Get");

            var User = _userService.Get();

            return Ok(User);
        }

        [HttpGet("TEST")]

        public ActionResult Test()
        {

            var mail = _mailer.Send();
            return Ok("");
        }
    }
}
