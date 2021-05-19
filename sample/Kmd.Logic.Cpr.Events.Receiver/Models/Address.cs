using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kmd.Logic.Cpr.Events.Receiver.Models
{
    public class Address
    {
        public PostDistrict PostDistrict { get; set; }

        public Municipality Municipality { get; set; }

        public HouseNumber HouseNumber { get; set; }

        public string ByName { get; set; }

        public string ByWay { get; set; }

        public DateTime? Date { get; set; }

        public string AdvertisingProtected { get; set; }

        public DateTime? AdvertisingProtectedFrom { get; set; }
    }
}
