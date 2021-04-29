using System;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Client.Models;

namespace Kmd.Logic.Cpr.Client
{
    public static class FakeProviderExtensions
    {
        /// <summary>
        /// Creates fake provider configuration.
        /// </summary>
        /// <param name="cprClient">Cpr client.</param>
        /// <param name="name">Name of the configuration.</param>
        /// <returns>Created configuration.</returns>
        public static async Task<FakeProviderConfigurationModel> CreateFakeProviderConfiguration(this CprClient cprClient, string name)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.CreateFakeProviderConfigurationWithHttpMessagesAsync(
                cprClient.GetOptions().SubscriptionId,
                name).ConfigureAwait(false);

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
        /// Updates fake provider configuration.
        /// </summary>
        /// <param name="cprClient">Cpr client.</param>
        /// <param name="configurationId">Id of the configuration.</param>
        /// <param name="name">Name of the configuration.</param>
        /// <returns>Updated configuration.</returns>
        public static async Task<FakeProviderConfigurationModel> UpdateFakeProviderConfiguration(
            this CprClient cprClient,
            Guid configurationId,
            string name)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.UpdateFakeProviderConfigurationWithHttpMessagesAsync(
                cprClient.GetOptions().SubscriptionId,
                configurationId,
                name).ConfigureAwait(false);

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