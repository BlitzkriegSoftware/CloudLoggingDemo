using GenerateLogs.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace GenerateLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();

            // Build DI Stack inc. Logging, Configuration, and Application
            ConfigureServices(serviceCollection, args);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Run the log generator
            var logmaker = serviceProvider.GetService<ILogMakerService>();
            logmaker.Run();
        }

        private static void ConfigureServices(IServiceCollection services, string[] args)
        {
            // Configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddEnvironmentVariables();
            if (args != null && args.Length > 0)
            {
                configurationBuilder.AddCommandLine(args);
            }
            var config = configurationBuilder.Build();
            services.AddSingleton(config);

            // Logging
            services.AddLogging(loggingBuilder => {
                // This line must be 1st
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);

                // Console is generically cloud friendly
                loggingBuilder.AddConsole();

                // Handy on Windows O/S, Azure, etc.
                loggingBuilder.AddDebug();
            });

            // App to run
            services.AddTransient<ILogMakerService,LogMakerService>();
        }
    }
}
