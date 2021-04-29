using System;
using System.IO;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Client.Models;

namespace Kmd.Logic.Cpr.Client
{
    public static class ServicePlatformProviderExtensions
    {
        public static async Task<ServicePlatformProviderConfiguration> CreateServicePlatformConfiguration(
            this CprClient cprClient,
            string name,
            ProviderEnvironment environment,
            Stream certificate,
            string certificatePassword = default,
            string serviceAgreementUuid = default,
            string userSystemUuid = default,
            string userUuid = default)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.CreateServicePlatformConfigurationWithHttpMessagesAsync(
                cprClient.GetOptions().SubscriptionId,
                name,
                environment.ToString(),
                certificate,
                certificatePassword,
                serviceAgreementUuid,
                userSystemUuid,
                userUuid).ConfigureAwait(false);

            switch (response.Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return response.Body;

                default:
                    throw new CprConfigurationException(response.Response?.ReasonPhrase ?? "Provider configuration creation failed");
            }
        }

        public static async Task<ServicePlatformProviderConfiguration> UpdateServicePlatformConfiguration(
            this CprClient cprClient,
            Guid configurationId,
            string name,
            ProviderEnvironment environment,
            Stream certificate,
            string certificatePassword = default,
            string serviceAgreementUuid = default,
            string userSystemUuid = default,
            string userUuid = default)
        {
            if (cprClient == null)
            {
                throw new ArgumentException("Client cannot be null", nameof(cprClient));
            }

            var client = cprClient.CreateClient();

            using var response = await client.UpdateServicePlatformConfigurationWithHttpMessagesAsync(
                cprClient.GetOptions().SubscriptionId,
                configurationId,
                name,
                environment.ToString(),
                certificate,
                certificatePassword,
                serviceAgreementUuid,
                userSystemUuid,
                userUuid).ConfigureAwait(false);

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