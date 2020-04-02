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

        public DateTime DateFrom { get; set; } = DateTime.Today.AddMonths(-2);

        public DateTime DateTo { get; set; } = DateTime.Today;

        public int PageNo { get; set; } = 1;

        public int PageSize { get; set; } = 1;
    }
}