using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor
{
    class JIRADataExtractor
    {
        static void Main(string[] args)
        {
            SetupStaticLogger();
            LastCompletedSprint jIRA = new LastCompletedSprint(email, apiToken, baseURL);
            jIRA.sprints(42);
        }

        private static void SetupStaticLogger()
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environmentName}.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
