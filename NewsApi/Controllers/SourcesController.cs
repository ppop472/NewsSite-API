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

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourcesController : ControllerBase
    {
        private readonly INewsItemsService _newsItemsService;
        private readonly ISourcesService _sourceService;
        private readonly ILogger<NewsItemController> _logger;
        public SourcesController(ILogger<NewsItemController> logger, ISourcesService sourceService)
        {
            _logger = logger;
            _sourceService = sourceService;
        }

        [HttpGet]
        public ActionResult ReadUser()
        {
            _logger.LogDebug("Api Sources Called.");

            _logger.LogDebug("Calling Sources.Business Read");

            var Sources = _sourceService.Read();

            return Ok(Sources);
        }
    }
}
