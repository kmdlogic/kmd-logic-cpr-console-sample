// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Kmd.Logic.Cpr.ConsoleSample.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class IFormFile
    {
        /// <summary>
        /// Initializes a new instance of the IFormFile class.
        /// </summary>
        public IFormFile()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IFormFile class.
        /// </summary>
        public IFormFile(string contentType = default(string), string contentDisposition = default(string), IDictionary<string, IList<string>> headers = default(IDictionary<string, IList<string>>), long? length = default(long?), string name = default(string), string fileName = default(string))
        {
            ContentType = contentType;
            ContentDisposition = contentDisposition;
            Headers = headers;
            Length = length;
            Name = name;
            FileName = fileName;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "contentDisposition")]
        public string ContentDisposition { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "headers")]
        public IDictionary<string, IList<string>> Headers { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public long? Length { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

    }
}
