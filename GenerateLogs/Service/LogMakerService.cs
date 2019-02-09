using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GenerateLogs.Service
{
    public class LogMakerService : ILogMakerService
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _config;

        // Notice the new method requires ILogger of T, not ILogger
        public LogMakerService(ILogger<LogMakerService> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public void Run()
        {
            for(int i=0; i<Loops; i++)
            {
                DumpConfig();
                MakeLogs();
                Thread.Sleep(Wait);
            }
        }

        private void DumpConfig()
        {
            foreach(var c in _config.AsEnumerable())
            {
                _logger.LogDebug("{0}: {1}", c.Key, c.Value);
            }
        }

        private void MakeLogs()
        {
            var ex0 = new StackOverflowException("Faux Overflow");
            _logger.LogCritical(ex0, "{0}", ex0.Message);

            var ex1 = new InvalidOperationException("This is a test");
            _logger.LogError(ex1, "{0}", ex1.Message);

            var ex2 = new ArgumentOutOfRangeException("fake", -1, "Must be 1..100");
            _logger.LogWarning(ex2, "{0}", ex2.Message);

            _logger.LogInformation("This is information");

            _logger.LogDebug("Debug");
        }

        /// <summary>
        /// Number of loops
        /// </summary>
        public int Loops
        {
            get
            {
                int loop = 1;
                loop = _config.GetValue<int>("loop");
                return loop;
            }
        }

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int Wait
        {
            get
            {
                int wait = 1000;
                wait = _config.GetValue<int>("wait");
                return wait;
            }
        }
    }
}
