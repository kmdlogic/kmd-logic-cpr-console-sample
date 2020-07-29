// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Kmd.Logic.Cpr.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class CitizenResponse
    {
        /// <summary>
        /// Initializes a new instance of the CitizenResponse class.
        /// </summary>
        public CitizenResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CitizenResponse class.
        /// </summary>
        public CitizenResponse(System.Guid id, bool nameAndAddressProtection, string firstName = default(string), string lastName = default(string), string cpr = default(string), string maritalStatus = default(string), string status = default(string), IList<Parent> parents = default(IList<Parent>), IList<LegalGuardian> legalGuardians = default(IList<LegalGuardian>), IList<Address> addresses = default(IList<Address>), IList<string> citizenships = default(IList<string>))
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Cpr = cpr;
            MaritalStatus = maritalStatus;
            Status = status;
            Parents = parents;
            LegalGuardians = legalGuardians;
            NameAndAddressProtection = nameAndAddressProtection;
            Addresses = addresses;
            Citizenships = citizenships;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cpr")]
        public string Cpr { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "maritalStatus")]
        public string MaritalStatus { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "parents")]
        public IList<Parent> Parents { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "legalGuardians")]
        public IList<LegalGuardian> LegalGuardians { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "nameAndAddressProtection")]
        public bool NameAndAddressProtection { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "addresses")]
        public IList<Address> Addresses { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "citizenships")]
        public IList<string> Citizenships { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}