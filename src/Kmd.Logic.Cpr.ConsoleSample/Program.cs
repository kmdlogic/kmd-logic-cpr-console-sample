using Kmd.Logic.Cpr.ConsoleSample.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kmd.Logic.Cpr.ConsoleSample
{
    internal class Program
    {
        private const string CprSampleNumber = "0101015084";

        private static async Task Main(string[] args)
        {
            InitLogger();

            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build()
                    .Get<AppConfiguration>();

                await Run(config);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Caught a fatal unhandled exception");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static async Task Run(AppConfiguration configuration)
        {
            var configurationValidation = new ConfigurationValidator(configuration).Validate();
            if (configurationValidation == ConfigurationValidator.Result.Invalid)
            {
                return;
            }

            var environment = configuration.LogicEnvironments.FirstOrDefault(e => e.Name == configuration.LogicEnvironmentName);

            if (environment == null)
            {
                Log.Error("Settings for `{LogicEnvironmentName}` are missing in `appsettings.json`.", configuration.LogicEnvironmentName);
                return;
            }

            var client = new CprClient(new TokenCredentials(new TokenProvider(configuration, environment)))
            {
                BaseUri = environment.ApiRootUri
            };

            if (configuration.Cpr.ConfigurationId == null)
            {
                var configs = await client.GetAllCprConfigurationsAsync(configuration.LogicAccount.SubscriptionId.Value);

                if (configs == null || configs.Count == 0)
                {
                    Log.Error("There are no CPR configurations defined for this subscription");
                    return;
                }
                else if (configs.Count > 1)
                {
                    Log.Error("There is more than one CPR configuration defined for this subscription");
                    return;
                }
            }

            var citizen = await client.GetByCprAsync(
                subscriptionId: configuration.LogicAccount.SubscriptionId.Value,
                cpr: CprSampleNumber,
                configurationId: configuration.Cpr.ConfigurationId);

            Log.Information("Citizen data: {@Citizen}", citizen);
        }
    }
}
