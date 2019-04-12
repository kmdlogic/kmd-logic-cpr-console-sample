using Serilog;

namespace Kmd.Logic.Cpr.ConsoleSample
{
    internal class ConfigurationValidator
    {
        private readonly AppConfiguration _configuration;

        public ConfigurationValidator(AppConfiguration configuration)
        {
            _configuration = configuration;
        }

        public enum Result { Valid, Invalid }

        public Result Validate()
        {
            if (_configuration.LogicAccount == null
                || string.IsNullOrWhiteSpace(_configuration.LogicAccount?.ClientId)
                || string.IsNullOrWhiteSpace(_configuration.LogicAccount?.ClientSecret)
                || _configuration.LogicAccount?.SubscriptionId == null)
            {
                Log.Error("Invalid `LogicAccount` configuration. Please provide proper information to `appsettings.json`. Current data are: {@LogicAccount}",
                    _configuration.LogicAccount);

                return Result.Invalid;
            }

            if (_configuration.Cpr.ConfigurationId == null)
            {
                Log.Error("Invalid `CPR Service` configuration. Please provide `configurationId`.");
                return Result.Invalid;
            }

            return Result.Valid;
        }
    }
}