using System;
using Newtonsoft.Json;

namespace Kmd.Logic.Cpr.Events.Receiver.Models
{
    public class CprEventListViewModel
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DateTime Time { get; set; }

        public CprEvent CprEvent { get; set; }

        public string Json
        {
            get
            {
                return this.CprEvent != null ? JsonConvert.SerializeObject(this.CprEvent, Formatting.Indented) : string.Empty;
            }
        }
    }
}
