using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Client.Models;
using Kmd.Logic.Identity.Authorization;
using Microsoft.Rest;

namespace Kmd.Logic.Cpr.Client
{
    /// <summary>
    /// Get the details of a citizen from the CPR.
    /// </summary>
    /// <remarks>
    /// To access the CPR you:
    /// - Create a Logic subscription
    /// - Have a client credential issued for the Logic platform
    /// - Create a CPR configuration for the distribution service being used.
    /// </remarks>
    [SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "HttpClient is not owned by this class.")]
    public sealed class CprClient
    {
        private readonly HttpClient httpClient;
        private readonly CprOptions options;
        private readonly LogicTokenProviderFactory tokenProviderFactory;

        private InternalClient internalClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CprClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use. The caller is expected to manage this resource and it will not be disposed.</param>
        /// <param name="tokenProviderFactory">The Logic access token provider factory.</param>
        /// <param name="options">The required configuration options.</param>
        public CprClient(HttpClient httpClient, LogicTokenProviderFactory tokenProviderFactory, CprOptions options)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.tokenProviderFactory = tokenProviderFactory ?? throw new ArgumentNullException(nameof(tokenProviderFactory));
        }

        /// <summary>
        /// Get the details of a citizen from the CPR register.
        /// </summary>
        /// <param name="cpr">The CPR number.</param>
        /// <returns>The citizen details or null if the CPR number isn't valid.</returns>
        /// <exception cref="ValidationException">Missing cpr number.</exception>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        /// <exception cref="CprConfigurationException">Invalid CPR configuration details.</exception>
        public async Task<Citizen> GetCitizenByCprAsync(string cpr)
        {
            var client = this.CreateClient();

            using (var response = await client.GetByCprWithHttpMessagesAsync(
                                subscriptionId: this.options.SubscriptionId,
                                cpr: cpr,
                                configurationId: this.options.CprConfigurationId).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return (Citizen)response.Body;

                    case System.Net.HttpStatusCode.NotFound:
                        return null;

                    default:
                        throw new CprConfigurationException("Invalid configuration provided to access CPR service", response.Body as string);
                }
            }
        }

        /// <summary>
        /// Get the details of a citizen from the CPR register by their identifier.
        /// </summary>
        /// <param name="id">The citizen identifier.</param>
        /// <returns>The citizen details or null if the identifier isn't valid.</returns>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        /// <exception cref="CprConfigurationException">Invalid CPR configuration details.</exception>
        public async Task<Citizen> GetCitizenByIdAsync(Guid id)
        {
            var client = this.CreateClient();

            using (var response = await client.GetByIdWithHttpMessagesAsync(
                                subscriptionId: this.options.SubscriptionId,
                                id: id,
                                configurationId: this.options.CprConfigurationId).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return (Citizen)response.Body;

                    case System.Net.HttpStatusCode.NotFound:
                        return null;

                    default:
                        throw new CprConfigurationException("Invalid configuration provided to access CPR service", response.Body as string);
                }
            }
        }

        /// <summary>
        /// Get the CPR configurations for the Logic subscription.
        /// </summary>
        /// <returns>The list of CPR configurations.</returns>
                /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        public async Task<IList<CprProviderConfigurationModel>> GetAllCprConfigurationsAsync()
        {
            var client = this.CreateClient();

            return await client.GetAllCprConfigurationsAsync(subscriptionId: this.options.SubscriptionId).ConfigureAwait(false);
        }

        private InternalClient CreateClient()
        {
            if (this.internalClient != null)
            {
                return this.internalClient;
            }

            var tokenProvider = this.tokenProviderFactory.GetProvider(this.httpClient);

            this.internalClient = new InternalClient(new TokenCredentials(tokenProvider))
            {
                BaseUri = this.options.CprServiceUri,
            };

            return this.internalClient;
        }
    }
}