using System;
using System.Collections.Generic;
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
    public sealed class CprClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly CprOptions _options;
        private readonly ITokenProviderFactory _tokenProviderFactory;

        private InternalClient _internalClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CprClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use. The caller is expected to manage this resource and it will not be disposed.</param>
        /// <param name="tokenProviderFactory">The Logic access token provider factory.</param>
        /// <param name="options">The required configuration options.</param>
        public CprClient(HttpClient httpClient, ITokenProviderFactory tokenProviderFactory, CprOptions options)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this._options = options ?? throw new ArgumentNullException(nameof(options));
            this._tokenProviderFactory = tokenProviderFactory ?? throw new ArgumentNullException(nameof(tokenProviderFactory));
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
        public async Task<CitizenResponse> GetCitizenByCprAsync(string cpr)
        {
            var client = this.CreateClient();

            using (var response = await client.GetByCprWithHttpMessagesAsync(
                                subscriptionId: this._options.SubscriptionId,
                                cpr: cpr,
                                configurationId: this._options.CprConfigurationId).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return (CitizenResponse)response.Body;

                    case System.Net.HttpStatusCode.NotFound:
                        return null;

                    default:
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

        /// <summary>
        /// Get more details of a citizen from the CPR register.
        /// </summary>
        /// <param name="cpr">The CPR number.</param>
        /// <returns>The citizen details or null if the CPR number isn't valid.</returns>
        /// <exception cref="ValidationException">Missing cpr number.</exception>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        /// <exception cref="CprConfigurationException">Invalid CPR configuration details.</exception>
        public async Task<CitizenDetailedResponse> GetCitizenDetailsByCprAsync(string cpr)
        {
            var client = this.CreateClient();

            using (var response = await client.GetCprDetailsByCprWithHttpMessagesAsync(
                                subscriptionId: this._options.SubscriptionId,
                                cpr: cpr,
                                configurationId: this._options.CprConfigurationId).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return (CitizenDetailedResponse)response.Body;

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
        public async Task<CitizenResponse> GetCitizenByIdAsync(Guid id)
        {
            var client = this.CreateClient();

            using (var response = await client.GetByIdWithHttpMessagesAsync(
                                subscriptionId: this._options.SubscriptionId,
                                id: id,
                                configurationId: this._options.CprConfigurationId).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return response.Body as CitizenResponse;

                    case System.Net.HttpStatusCode.NotFound:
                        return null;

                    default:
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

        /// <summary>
        /// Get more details of a citizen from the CPR register by their identifier.
        /// </summary>
        /// <param name="id">The citizen identifier.</param>
        /// <returns>The citizen details or null if the identifier isn't valid.</returns>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        /// <exception cref="LogicTokenProviderException">Unable to issue an authorization token.</exception>
        /// <exception cref="CprConfigurationException">Invalid CPR configuration details.</exception>
        public async Task<CitizenDetailedResponse> GetCitizenDetailsByIdAsync(Guid id)
        {
            var client = this.CreateClient();

            using (var response = await client.GetCprDetailsByIdWithHttpMessagesAsync(
                                subscriptionId: this._options.SubscriptionId,
                                id: id,
                                configurationId: this._options.CprConfigurationId).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return response.Body as CitizenDetailedResponse;

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

            return await client.GetAllCprConfigurationsAsync(subscriptionId: this._options.SubscriptionId).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes for CPR events by CPR number.
        /// </summary>
        /// <param name="cpr">The CPR number.</param>
        /// <returns>The Saved CprPersonId.</returns>
        /// <exception cref="ValidationException"> When subscriptionId or CPR number is null.</exception>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        public async Task<bool> SubscribeByCprAsync(string cpr)
        {
           var client = this.CreateClient();

           using (var response = await client.SubscribeByCprWithHttpMessagesAsync(
                  subscriptionId: this._options.SubscriptionId,
                  cpr: cpr,
                  request: new CprSubscriptionRequest(this._options.CprConfigurationId)).ConfigureAwait(false))
             {
                return response.Response.IsSuccessStatusCode;
             }
        }

         /// <summary>
        /// Subscribes for CPR events by PersonId.
        /// </summary>
        /// <param name="id">The CPR PersonID.</param>
        /// <returns>The Saved CprPersonId.</returns>
        /// <exception cref="ValidationException"> When subscriptionId or CPR PersonID is null.</exception>
        /// <exception cref="SerializationException">Unable process the service response.</exception>
        public async Task<bool> SubscribeByIdAsync(Guid id)
        {
             var client = this.CreateClient();

             using (var response = await client.SubscribeByIdWithHttpMessagesAsync(
                  subscriptionId: this._options.SubscriptionId,
                  id: id,
                  request: new CprSubscriptionRequest(this._options.CprConfigurationId)).ConfigureAwait(false))
             {
                return response.Response.IsSuccessStatusCode;
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
                  subscriptionId: this._options.SubscriptionId,
                  cpr: cpr,
                  configurationId: this._options.CprConfigurationId).ConfigureAwait(false))
            {
                return response.Response.IsSuccessStatusCode;
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
                   subscriptionId: this._options.SubscriptionId,
                   id: id,
                   configurationId: this._options.CprConfigurationId).ConfigureAwait(false))
            {
                return response.Response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Gets citizen events for the nominated period.
        /// </summary>
        /// <param name="dateFom">Query events from this date and time.</param>
        /// <param name="dateTo">Query events to this date and time.</param>
        /// <param name="pageNo">The page number to query, starting at 1.</param>
        /// <param name="pageSize">The maximum number of results to return.</param>
        /// <returns>List of citizen records.</returns>
        public async Task<IList<CitizenEvent>> GetAllCprEventsAsync(DateTime dateFom, DateTime dateTo, int pageNo, int pageSize)
        {
            var client = this.CreateClient();

            using (var response = await client.GetEventsWithHttpMessagesAsync(
                subscriptionId: this._options.SubscriptionId,
                dateFrom: dateFom,
                dateTo: dateTo,
                configurationId: this._options.CprConfigurationId,
                pageNo: pageNo,
                pageSize: pageSize).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return response.Body as IList<CitizenEvent>;

                    case System.Net.HttpStatusCode.NotFound:
                        return null;

                    default:
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

        /// <summary>
        /// Gets Subscribed citizen events for the nominated period.
        /// </summary>
        /// <param name="dateFom">Query events from this date and time.</param>
        /// <param name="dateTo">Query events to this date and time.</param>
        /// <param name="pageNo">The page number to query, starting at 1.</param>
        /// <param name="pageSize">The maximum number of results to return.</param>
        /// <returns>Subscribed citizen records.</returns>
        public async Task<SubscribedCitizenEvents> GetSubscribedCprEventsAsync(DateTime dateFom, DateTime dateTo, int pageNo, int pageSize)
        {
            var client = this.CreateClient();

            using (var response = await client.GetSubscribedEventsWithHttpMessagesAsync(
                subscriptionId: this._options.SubscriptionId,
                dateFrom: dateFom,
                dateTo: dateTo,
                configurationId: this._options.CprConfigurationId,
                pageNo: pageNo,
                pageSize: pageSize).ConfigureAwait(false))
            {
                switch (response.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return response.Body as SubscribedCitizenEvents;

                    case System.Net.HttpStatusCode.NotFound:
                        return null;

                    default:
                        throw new CprConfigurationException(response.Body as string ?? "Invalid configuration provided to access CPR service");
                }
            }
        }

        internal CprOptions GetOptions()
        {
            return this._options;
        }

        internal InternalClient CreateClient()
        {
            if (this._internalClient != null)
            {
                return this._internalClient;
            }

            var tokenProvider = this._tokenProviderFactory.GetProvider(this._httpClient);

            this._internalClient = new InternalClient(new TokenCredentials(tokenProvider))
            {
                BaseUri = this._options.CprServiceUri,
            };

            return this._internalClient;
        }

        public void Dispose()
        {
            this._httpClient?.Dispose();
            this._tokenProviderFactory?.Dispose();
            this._internalClient?.Dispose();
        }
    }
}