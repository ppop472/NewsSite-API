using Microsoft.AspNetCore.Mvc;
using News.Business.Interfaces;
using News.Business.Models;
using Serilog.AspNetCore;
using Serilog.Configuration;
using Serilog.Settings.Configuration;
using Serilog.Sinks.File;
using Serilog.Sinks.Async;
using News.Repositories.Models;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsItemController : ControllerBase
    {
        private readonly INewsItemsService _newsItemsService;
        private readonly IUserService _userService;
        private readonly ILogger<NewsItemController> _logger;


        public NewsItemController(INewsItemsService newsItemsService, ILogger<NewsItemController> logger, IUserService userService)
        {
            _newsItemsService = newsItemsService;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<NewsItemDto>> AllNews()
        {
            _logger.LogDebug("Api AllNews Called.");

            _logger.LogDebug("Calling NewsItemService.Get");

            var newsItems = _newsItemsService.Get();
            return Ok(newsItems);
        }

        [HttpGet("{id}")]
        public ActionResult NewsWithId(int id)
        {
            _logger.LogDebug("Api NewsWithId Called.");

            _logger.LogDebug("Calling NewsItemService.Get(id)");

            var newsItem = _newsItemsService.Get(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return Ok(newsItem);
        } 
    }
}