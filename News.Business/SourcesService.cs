using Microsoft.Extensions.Logging;
using News.Business.Interfaces;
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
    public class SourcesService : ISourcesService
    {
        private readonly ISourceRepository _sourceRepository;
        private readonly ILogger<User> _logger;

        public SourcesService(ISourceRepository sourceRepository, ILogger<User> logger)
        {
            _sourceRepository = sourceRepository;
            _logger = logger;
        }

        public List<Sources> Read()
        {

            _logger.LogDebug("Calling User.Respository Read.");
            var sources = _sourceRepository.Get();

            return (List<Sources>)sources;
        }
    }
}
