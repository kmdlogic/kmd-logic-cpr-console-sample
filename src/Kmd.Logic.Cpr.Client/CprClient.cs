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

        public CprSubscriptionRequest CprSubscription { get; set; } = new CprSubscriptionRequest(Guid.Parse("5e3d9df6-d082-467a-88bd-dca56edc7328"));

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

#pragma warning disable CS0618 // Type or member is obsolete
            if (string.IsNullOrEmpty(this.tokenProviderFactory.DefaultAuthorizationScope))
            {
                this.tokenProviderFactory.DefaultAuthorizationScope = "https://logicidentityprod.onmicrosoft.com/bb159109-0ccd-4b08-8d0d-80370cedda84/.default";
            }
#pragma warning restore CS0618 // Type or member is obsolete
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
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
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
        public async Task<object> GetCitizenByIdAsync(Guid id)
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
                        return (object)response.Body;

                    case System.Net.HttpStatusCode.NotFound:
                        return null;

                    default:
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
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

         /// <summary>
        /// Subscribes for CPR events by CPR number.
        /// </summary>
        /// <param name="cpr">The CPR number.</param>
        /// <returns>The Saved CprPersonId.</returns>
        /// <exception cref="ValidationException"> When subscriptionId or CPR number is null.</exception>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        public async Task<Guid> SubscribeByCprAsync(string cpr)
        {
           var client = this.CreateClient();

           using (var response = await client.SubscribeByCprWithHttpMessagesAsync(
                  subscriptionId: this.options.SubscriptionId,
                  cpr: cpr,
                  request: this.CprSubscription).ConfigureAwait(false))
             {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return (Guid)response.Body;

                    case System.Net.HttpStatusCode.BadRequest:
                        return Guid.Empty;

                    default:
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

         /// <summary>
        /// Subscribes for CPR events by PersonId.
        /// </summary>
        /// <param name="id">The CPR PersonID.</param>
        /// <returns>The Saved CprPersonId.</returns>
        /// <exception cref="ValidationException"> When subscriptionId or CPR PersonID is null.</exception>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        public async Task<Guid> SubscribeByIdAsync(Guid id)
        {
             var client = this.CreateClient();

             using (var response = await client.SubscribeByIdWithHttpMessagesAsync(
                subscriptionId: this.options.SubscriptionId,
                id: id,
                request: this.CprSubscription).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return (Guid)response.Body;

                    case System.Net.HttpStatusCode.BadRequest:
                        return Guid.Empty;

                    default:
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

         /// <summary>
        /// UnSubscribe for CPR events by CPR number.
        /// </summary>
        /// <param name="cpr">The CPR number.</param>
        /// <returns>True in case of unsubscribe.</returns>
        /// <exception cref="ValidationException"> When subscriptionId or CPR number is null.</exception>
        public async Task<bool> UnsubscribeByCprAsync(string cpr)
        {
            var client = this.CreateClient();

            using (var response = await client.UnsubscribeByCprWithHttpMessagesAsync(
                 subscriptionId: this.options.SubscriptionId,
                 cpr: cpr,
                 configurationId: this.options.CprConfigurationId).ConfigureAwait(false))
             {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return true;

                    case System.Net.HttpStatusCode.NotFound:
                        return false;

                    default:
                        throw new CprConfigurationException(response.Body.ToString() as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

        /// <summary>
        /// Unsubscribes for CPR events by PersonId.
        /// </summary>
        /// <param name="id">The CPR number.</param>
        /// <returns>True in case of unsubscribe.</returns>
        /// <exception cref="ValidationException"> When subscriptionId or CPR number is null.</exception>
        public async Task<bool> UnsubscribeByIdAsync(Guid id)
        {
            var client = this.CreateClient();

            using (var response = await client.UnsubscribeByIdWithHttpMessagesAsync(
                 subscriptionId: this.options.SubscriptionId,
                 id: id,
                 configurationId: this.options.CprConfigurationId).ConfigureAwait(false))
             {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return true;

                    case System.Net.HttpStatusCode.NotFound:
                        return false;

                    default:
                        throw new CprConfigurationException(response.Body.ToString() as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

        /// <summary>
        /// Gets citizen events for the nominated period.
        /// </summary>
        /// <returns>List of citizen records</returns>
        public async Task<object> GetAllCprEvents()
        {
            var client = this.CreateClient();

            return await client.GetEventsWithHttpMessagesAsync(
                subscriptionId: this.options.SubscriptionId,
                dateFrom: DateTime.Today.AddMonths(-2),
                dateTo: DateTime.Today,
                configurationId: this.options.CprConfigurationId,
                pageNo: 1,
                pageSize: 1).ConfigureAwait(false);
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