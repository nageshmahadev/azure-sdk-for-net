// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.0.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.WebSites.Models
{
    using Azure;
    using Management;
    using WebSites;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// String dictionary resource.
    /// </summary>
    public partial class ConnectionStringDictionary : Resource
    {
        /// <summary>
        /// Initializes a new instance of the ConnectionStringDictionary class.
        /// </summary>
        public ConnectionStringDictionary() { }

        /// <summary>
        /// Initializes a new instance of the ConnectionStringDictionary class.
        /// </summary>
        /// <param name="location">Resource Location.</param>
        /// <param name="id">Resource Id.</param>
        /// <param name="name">Resource Name.</param>
        /// <param name="kind">Kind of resource.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="tags">Resource tags.</param>
        /// <param name="properties">Connection strings.</param>
        public ConnectionStringDictionary(string location, string id = default(string), string name = default(string), string kind = default(string), string type = default(string), IDictionary<string, string> tags = default(IDictionary<string, string>), IDictionary<string, ConnStringValueTypePair> properties = default(IDictionary<string, ConnStringValueTypePair>))
            : base(location, id, name, kind, type, tags)
        {
            Properties = properties;
        }

        /// <summary>
        /// Gets or sets connection strings.
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public IDictionary<string, ConnStringValueTypePair> Properties { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public override void Validate()
        {
            base.Validate();
            if (Properties != null)
            {
                foreach (var valueElement in Properties.Values)
                {
                    if (valueElement != null)
                    {
                        valueElement.Validate();
                    }
                }
            }
        }
    }
}
