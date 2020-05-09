using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace GenerateLogs.Service
{
    public class LogMakerService : ILogMakerService
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfigurationRoot _config;
        private readonly BlitzkriegSoftware.SecureRandomLibrary.SecureRandom Dice = new BlitzkriegSoftware.SecureRandomLibrary.SecureRandom();


        // Notice the new method requires ILogger of T, not ILogger
        public LogMakerService(ILoggerFactory loggerFactory, IConfigurationRoot config)
        {
            this._loggerFactory = loggerFactory;
            this._logger = loggerFactory.CreateLogger<LogMakerService>();
            this._config = config;
        }

        public void Run()
        {
            this.DumpConfig(this._loggerFactory.CreateLogger("ConfigDumper"));

            for (int i=0; i< this.Loops; i++)
            {
                this.MakeLogs();
                Thread.Sleep(this.Wait);
            }
        }

        private void DumpConfig(ILogger logger)
        {
            foreach(var c in this._config.AsEnumerable())
            {
                logger.LogDebug("{0}: {1}", c.Key, c.Value);
            }
        }

        private void MakeLogs()
        {
            var index = this.Dice.Next(1, 100);
            Exception ex0;

            switch(index)
            {
                case int n when (n < 10):
                     ex0 = new Models.RequiredConfigurationMissingException("ConfigKeyA", "Application will not run correctly!");
                    this._logger.LogCritical(ex0, "{0}", ex0.ToString());
                    break;
                case int n when (n >= 10 && n < 20):
                    ex0 = new Models.RequiredConfigurationBadValueException("ConfigKeyA", -20, "Application will not run correctly until this key has a valid value supplied (expected (int) 1-1000)");
                    this._logger.LogCritical(ex0, "{0}", ex0.ToString());
                    break;
                case int n when (n >= 20 && n < 40):
                    ex0 = new InvalidOperationException("The code did a bad thing, this is why... Process will not crash, but a unit of work may have been lost");
                    this._logger.LogError(ex0, "{0}", ex0.ToString());
                    break;
                case int n when (n >= 40 && n < 50):
                    ex0 = new TimeoutException("It took a couple of tries, but it worked eventually");
                    this._logger.LogWarning(ex0, "{0}", ex0.ToString());
                    break;
                case int n when (n >= 50 && n < 70):
                    this._logger.LogInformation("This is information for **Business** Users, do not mis-use this for developer messages...");
                    break;
                case int n when (n >= 70 && n < 80):
                    this._logger.LogDebug("Debug (coarse debugging) for Developers");
                    break;
                default: this._logger.LogTrace("Trace (very detailed debugging) For Developers"); break;
            }
        }

        /// <summary>
        /// Number of loops
        /// </summary>
        public int Loops
        {
            get
            {
                int loop = this._config.GetValue<int>("loop", 3);
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
                int wait = this._config.GetValue<int>("wait", 200);
                return wait;
            }
        }

    }
}
