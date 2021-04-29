using System;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Client.Models;

namespace Kmd.Logic.Cpr.Client
{
    public static class ExperianExtensions
    {
        /// <summary>
        /// Creates fake provider configuration.
        /// </summary>
        /// <param name="cprClient">Cpr client.</param>
        /// <param name="name">Name of the configuration.</param>
        /// <param name="environment">Experian environment.</param>
        /// <param name="callbackUrl">Url to endpoint to send events.</param>
        /// <returns>Created configuration.</returns>
        public static async Task<ExperianProviderConfigurationResponse> CreateExperianConfiguration(
            this CprClient cprClient,
            string name,
            ProviderEnvironment environment,
            Uri callbackUrl = default)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.CreateExperianConfigurationWithHttpMessagesAsync(
                    cprClient.GetOptions().SubscriptionId,
                    new ExperianConfigurationRequest(
                        name,
                        environment.ToString(),
                        callbackUrl?.ToString()))
                .ConfigureAwait(false);

            switch (response.Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return response.Body;

                case System.Net.HttpStatusCode.NotFound:
                    return null;

                default:
                    throw new CprConfigurationException(response.Response?.ReasonPhrase ?? "Provider configuration creation failed");
            }
        }

        /// <summary>
        /// Updates experian provider configuration.
        /// </summary>
        /// <param name="cprClient">Cpr client.</param>
        /// <param name="configurationId">Id of the configuration.</param>
        /// <param name="name">Name of the configuration.</param>
        /// <param name="environment">Experian environment.</param>
        /// <param name="callbackUrl">Url to endpoint to send events.</param>
        /// <returns>Updated configuration.</returns>
        public static async Task<ExperianProviderConfigurationResponse> UpdateExperianConfiguration(
            this CprClient cprClient,
            Guid configurationId,
            string name,
            ProviderEnvironment environment,
            Uri callbackUrl = default)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.UpdateExperianConfigurationWithHttpMessagesAsync(
                cprClient.GetOptions().SubscriptionId,
                configurationId,
                new ExperianConfigurationRequest(
                    name,
                    environment.ToString(),
                    callbackUrl?.ToString())).ConfigureAwait(false);

            switch (response.Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return response.Body;

                case System.Net.HttpStatusCode.NotFound:
                    throw new CprConfigurationException("Configuration not found");

                default:
                    throw new CprConfigurationException(response.Response?.ReasonPhrase ?? "Provider configuration update failed");
            }
        }
    }
}