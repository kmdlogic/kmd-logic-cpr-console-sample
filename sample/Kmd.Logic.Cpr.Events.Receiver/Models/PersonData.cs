using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kmd.Logic.Cpr.Events.Receiver.Models
{
    public class PersonData
    {
        public string MasterCardNumber { get; set; }

        public NameData NameData { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string CprStatus { get; set; }

        public string CreditWarning { get; set; }
    }
}
