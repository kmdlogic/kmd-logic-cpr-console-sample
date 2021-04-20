using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kmd.Logic.Cpr.Events.Receiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExperianEventController : ControllerBase
    {   
        [HttpPost]
        public ActionResult Post(ExperianEvent experianEvent)
        {   
            Log.Information(JsonConvert.SerializeObject(experianEvent));            
            return Ok();
        }
    }

    public class ExperianEvent
    {
        public string ReferenceNumber { get; set; }

        public string MessageId { get; set; }

        public string MessageType { get; set; }

        public ExperianPersonData PersonData { get; set; }

        public Address Address { get; set; }
    }

    public class ExperianPersonData
    {
        public string MasterCardNumber { get; set; }

        public NameData NameData { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string CprStatus { get; set; }

        public string CreditWarning { get; set; }
    }

    public class NameData
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Address
    {
        public PostDistrict PostDistrict { get; set; }

        public Municipality Municipality { get; set; }

        public HouseNumber HouseNumber { get; set; }

        public string ByName { get; set; }

        public string ByWay { get; set; }

        public DateTime? Date { get; set; }

        public string Advertisingprotected { get; set; }

        public DateTime? AdvetisingprotectedFrom { get; set; }
    }

    public class HouseNumber
    {
        public string FromNumber { get; set; }
    }

    public class PostDistrict
    {
        public string Name { get; set; }

        public string ZipCode { get; set; }
    }

    public class Municipality
    {
        public string Code { get; set; }
    }
}