using Kmd.Logic.Authentication;

namespace Kmd.Logic.Cpr.Client.Sample
{
    internal class AppConfiguration
    {
        public string CprNumber { get; set; } = "0101015084";

        public LogicTokenProviderOptions TokenProvider { get; set; } = new LogicTokenProviderOptions();

        public CprOptions Cpr { get; set; } = new CprOptions();
    }
}