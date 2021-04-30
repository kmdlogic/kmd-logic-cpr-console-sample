using System;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Kmd.Logic.Cpr.Events.Receiver.Controllers
{
#pragma warning disable SA1402 // File may only contain a single type
    [ApiController]
    [Route("[controller]")]
    public class ExperianEventController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post(ExperianEvent experianEvent)
        {
            Log.Information("Event received: {@Event}", experianEvent);
            return this.Ok();
        }
    }

    public class ExperianEvent
    {
        public string referenceNumber { get; set; }

        public string messageId { get; set; }

        public string messageType { get; set; }

        public ExperianPersonData personData { get; set; }

        public Address address { get; set; }
    }

    public class ExperianPersonData
    {
        public string masterCardNumber { get; set; }

        public NameData nameData { get; set; }

        public dateTime? dateOfBirth { get; set; }

        public string cprStatus { get; set; }

        public string creditWarning { get; set; }
    }

    public class NameData
    {
        public string firstName { get; set; }

        public string lastName { get; set; }
    }

    public class Address
    {
        public PostDistrict postDistrict { get; set; }

        public Municipality municipality { get; set; }

        public HouseNumber houseNumber { get; set; }

        public string byName { get; set; }

        public string byWay { get; set; }

        public dateTime? date { get; set; }

        public string advertisingProtected { get; set; }

        public dateTime? advetisingProtectedFrom { get; set; }
    }

    public class HouseNumber
    {
        public string fromNumber { get; set; }
    }

    public class PostDistrict
    {
        public string name { get; set; }

        public string zipCode { get; set; }
    }

    public class Municipality
    {
        public string code { get; set; }
    }
#pragma warning restore SA1402 // File may only contain a single type
}