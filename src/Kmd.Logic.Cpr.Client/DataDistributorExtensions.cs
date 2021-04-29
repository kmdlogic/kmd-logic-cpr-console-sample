using System;
using System.IO;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Client.Models;

namespace Kmd.Logic.Cpr.Client
{
    public static class DataDistributorExtensions
    {
        /// <summary>
        /// Creates Data Distributor provider configuration.
        /// </summary>
        /// <param name="cprClient">Cpr client.</param>
        /// <param name="name">Name of the configuration.</param>
        /// <param name="environment">Environment to use.</param>
        /// <param name="certificate">Stream with certificate.</param>
        /// <param name="password">Password to the certificate.</param>
        /// <returns>Created configuration.</returns>
        public static async Task<CprProviderConfiguration> CreateDataDistributorConfiguration(
            this CprClient cprClient,
            string name,
            ProviderEnvironment environment,
            Stream certificate,
            string password = null)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.CreateDataDistributorConfigurationWithHttpMessagesAsync(
                cprClient.GetOptions().SubscriptionId,
                name,
                environment.ToString(),
                certificate,
                password).ConfigureAwait(false);

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
        /// Updates Data Distributor provider configuration.
        /// </summary>
        /// <param name="cprClient">Cpr client.</param>
        /// <param name="configurationId">Id of the configuration to update.</param>
        /// <param name="name">Name of the configuration.</param>
        /// <param name="environment">Environment to use.</param>
        /// <param name="certificate">Stream with certificate.</param>
        /// <param name="password">Password to the certificate.</param>
        /// <returns>Updated configuration.</returns>
        public static async Task<CprProviderConfiguration> UpdateDataDistributorConfiguration(
            this CprClient cprClient,
            Guid configurationId,
            string name,
            ProviderEnvironment environment,
            Stream certificate,
            string password = null)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.UpdateDataDistributorConfigurationWithHttpMessagesAsync(
                cprClient.GetOptions().SubscriptionId,
                configurationId,
                name,
                environment.ToString(),
                certificate,
                password).ConfigureAwait(false);

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