using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Rest;
using Microsoft.Rest.Serialization;
using Microsoft.Azure;

namespace Microsoft.Azure.Management.Storage.Models
{
    /// <summary>
    /// </summary>
    public partial class CheckNameAvailabilityResponse
    {
        /// <summary>
        /// Gets a boolean value that indicates whether the name is available
        /// for you to use. If true, the name is available. If false, the
        /// name has already been taken or invalid and cannot be used.
        /// </summary>
        [JsonProperty(PropertyName = "nameAvailable")]
        public bool? NameAvailable { get; set; }

        /// <summary>
        /// Gets the reason that a storage account name could not be used. The
        /// Reason element is only returned if NameAvailable is false.
        /// </summary>
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Gets an error message explaining the Reason value in more detail.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Validate the object. Throws ArgumentException or ArgumentNullException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}