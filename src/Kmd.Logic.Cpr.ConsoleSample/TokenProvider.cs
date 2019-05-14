using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Rest;
using Serilog;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Kmd.Logic.Cpr.ConsoleSample
{
    internal class TokenProvider : ITokenProvider
    {
        private readonly AppConfiguration _configuration;
        private readonly LogicEnvironmentConfiguration _environment;
        private DateTime? _expiration;
        private TokenResponse _currentToken;

        public TokenProvider(AppConfiguration configuration, LogicEnvironmentConfiguration environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken)
        {
            if (_expiration == null || _expiration.Value < DateTime.Now)
            {
                Log.Debug("Token expired or doesn't exist.");
                _currentToken = null;
            }

            if (_currentToken == null)
            {
                Log.Information("Requesting auth token for account `{LogicAccount}` and subscription `{SubscriptionId}`",
                    _configuration.LogicAccount.ClientId, _configuration.LogicAccount.SubscriptionId);

                var expire = DateTime.Now;
                var token = await RequestToken(
                    _environment.AuthorizationServerTokenIssuerUri,
                    _configuration.LogicAccount.ClientId,
                    _environment.ScopeUri.ToString(),
                    _configuration.LogicAccount.ClientSecret);
                _expiration = expire.AddSeconds(token.expires_in);

                Log.Debug("Got token {@Token}", token);
                if (string.IsNullOrEmpty(token.access_token)) 
                {
                    throw new Exception("Unable to get a token from the token issuer");
                }

                _currentToken = token;
            }

            return new AuthenticationHeaderValue(_currentToken.token_type, _currentToken.access_token);
        }

        private static async Task<TokenResponse> RequestToken(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret)
        {
            HttpResponseMessage responseMessage;

            using (var client = new HttpClient())
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                var httpContent = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("scope", scope),
                        new KeyValuePair<string, string>("client_secret", clientSecret)
                    });
                tokenRequest.Content = httpContent;
                Log.Debug("Requesting an access token {@Request}", tokenRequest);
                responseMessage = await client.SendAsync(tokenRequest);
            }

            return await responseMessage.Content.ReadAsAsync<TokenResponse>();
        }

        internal class TokenResponse
        {
#pragma warning disable IDE1006 // Naming Styles
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public int ext_expires_in { get; set; }
            public string access_token { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }
    }
}