using System;

namespace Kmd.Logic.Cpr.ConsoleSample
{
    internal class AppConfiguration
    {
        public LogicEnvironmentConfiguration[] LogicEnvironments { get; set; }
        public string LogicEnvironmentName { get; set; }
        public LogicAccountConfiguration LogicAccount { get; set; }
    }

    internal class LogicEnvironmentConfiguration
    {
        public string Name { get; set; }
        public Uri AuthorizationServerTokenIssuerUri { get; set; }
        public Uri ScopeUri { get; set; }
        public Uri ApiRootUri { get; set; }
    }

    internal class LogicAccountConfiguration
    {
        public Guid? SubscriptionId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}