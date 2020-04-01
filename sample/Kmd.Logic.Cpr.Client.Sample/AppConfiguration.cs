using System;
using Kmd.Logic.Identity.Authorization;

namespace Kmd.Logic.Cpr.Client.Sample
{
    internal class AppConfiguration
    {
        public string CprNumber { get; set; } = "0101015084";

        public LogicTokenProviderOptions TokenProvider { get; set; } = new LogicTokenProviderOptions();

        public CprOptions Cpr { get; set; } = new CprOptions();

        public Guid CprPersonId { get; set; } = Guid.Parse("5e3d9df6-d082-467a-88bd-dca56edc7328");
    }
}