// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Common;
using Microsoft.WindowsAzure.Common.Internals;
using Microsoft.WindowsAzure.Management;
using Microsoft.WindowsAzure.Management.Models;

namespace Microsoft.WindowsAzure.Management
{
    /// <summary>
    /// The Service Management API includes operations for listing the
    /// available role sizes for VMs in your subscription.
    /// </summary>
    internal partial class RoleSizeOperations : IServiceOperations<ManagementClient>, Microsoft.WindowsAzure.Management.IRoleSizeOperations
    {
        /// <summary>
        /// Initializes a new instance of the RoleSizeOperations class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the service client.
        /// </param>
        internal RoleSizeOperations(ManagementClient client)
        {
            this._client = client;
        }
        
        private ManagementClient _client;
        
        /// <summary>
        /// Gets a reference to the
        /// Microsoft.WindowsAzure.Management.ManagementClient.
        /// </summary>
        public ManagementClient Client
        {
            get { return this._client; }
        }
        
        /// <summary>
        /// The List Role Sizes operation lists all of the role sizes that are
        /// valid for your subscription.
        /// </summary>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The List Role Sizes operation response.
        /// </returns>
        public async System.Threading.Tasks.Task<Microsoft.WindowsAzure.Management.Models.RoleSizeListResponse> ListAsync(CancellationToken cancellationToken)
        {
            // Validate
            
            // Tracing
            bool shouldTrace = CloudContext.Configuration.Tracing.IsEnabled;
            string invocationId = null;
            if (shouldTrace)
            {
                invocationId = Tracing.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                Tracing.Enter(invocationId, this, "ListAsync", tracingParameters);
            }
            
            // Construct URL
            string baseUrl = this.Client.BaseUri.AbsoluteUri;
            string url = "/" + (this.Client.Credentials.SubscriptionId != null ? this.Client.Credentials.SubscriptionId.Trim() : "") + "/rolesizes";
            // Trim '/' character from the end of baseUrl and beginning of url.
            if (baseUrl[baseUrl.Length - 1] == '/')
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            url = baseUrl + "/" + url;
            
            // Create HTTP transport objects
            HttpRequestMessage httpRequest = null;
            try
            {
                httpRequest = new HttpRequestMessage();
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = new Uri(url);
                
                // Set Headers
                httpRequest.Headers.Add("x-ms-version", "2014-05-01");
                
                // Set Credentials
                cancellationToken.ThrowIfCancellationRequested();
                await this.Client.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                
                // Send Request
                HttpResponseMessage httpResponse = null;
                try
                {
                    if (shouldTrace)
                    {
                        Tracing.SendRequest(invocationId, httpRequest);
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    httpResponse = await this.Client.HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                    if (shouldTrace)
                    {
                        Tracing.ReceiveResponse(invocationId, httpResponse);
                    }
                    HttpStatusCode statusCode = httpResponse.StatusCode;
                    if (statusCode != HttpStatusCode.OK)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        CloudException ex = CloudException.Create(httpRequest, null, httpResponse, await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                        if (shouldTrace)
                        {
                            Tracing.Error(invocationId, ex);
                        }
                        throw ex;
                    }
                    
                    // Create Result
                    RoleSizeListResponse result = null;
                    // Deserialize Response
                    cancellationToken.ThrowIfCancellationRequested();
                    string responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    result = new RoleSizeListResponse();
                    XDocument responseDoc = XDocument.Parse(responseContent);
                    
                    XElement roleSizesSequenceElement = responseDoc.Element(XName.Get("RoleSizes", "http://schemas.microsoft.com/windowsazure"));
                    if (roleSizesSequenceElement != null)
                    {
                        foreach (XElement roleSizesElement in roleSizesSequenceElement.Elements(XName.Get("RoleSize", "http://schemas.microsoft.com/windowsazure")))
                        {
                            RoleSizeListResponse.RoleSize roleSizeInstance = new RoleSizeListResponse.RoleSize();
                            result.RoleSizes.Add(roleSizeInstance);
                            
                            XElement nameElement = roleSizesElement.Element(XName.Get("Name", "http://schemas.microsoft.com/windowsazure"));
                            if (nameElement != null)
                            {
                                string nameInstance = nameElement.Value;
                                roleSizeInstance.Name = nameInstance;
                            }
                            
                            XElement labelElement = roleSizesElement.Element(XName.Get("Label", "http://schemas.microsoft.com/windowsazure"));
                            if (labelElement != null)
                            {
                                string labelInstance = labelElement.Value;
                                roleSizeInstance.Label = labelInstance;
                            }
                            
                            XElement coresElement = roleSizesElement.Element(XName.Get("Cores", "http://schemas.microsoft.com/windowsazure"));
                            if (coresElement != null)
                            {
                                int coresInstance = int.Parse(coresElement.Value, CultureInfo.InvariantCulture);
                                roleSizeInstance.Cores = coresInstance;
                            }
                            
                            XElement memoryInMbElement = roleSizesElement.Element(XName.Get("MemoryInMb", "http://schemas.microsoft.com/windowsazure"));
                            if (memoryInMbElement != null)
                            {
                                int memoryInMbInstance = int.Parse(memoryInMbElement.Value, CultureInfo.InvariantCulture);
                                roleSizeInstance.MemoryInMb = memoryInMbInstance;
                            }
                            
                            XElement supportedByWebWorkerRolesElement = roleSizesElement.Element(XName.Get("SupportedByWebWorkerRoles", "http://schemas.microsoft.com/windowsazure"));
                            if (supportedByWebWorkerRolesElement != null)
                            {
                                bool supportedByWebWorkerRolesInstance = bool.Parse(supportedByWebWorkerRolesElement.Value);
                                roleSizeInstance.SupportedByWebWorkerRoles = supportedByWebWorkerRolesInstance;
                            }
                            
                            XElement supportedByVirtualMachinesElement = roleSizesElement.Element(XName.Get("SupportedByVirtualMachines", "http://schemas.microsoft.com/windowsazure"));
                            if (supportedByVirtualMachinesElement != null)
                            {
                                bool supportedByVirtualMachinesInstance = bool.Parse(supportedByVirtualMachinesElement.Value);
                                roleSizeInstance.SupportedByVirtualMachines = supportedByVirtualMachinesInstance;
                            }
                            
                            XElement maxDataDiskCountElement = roleSizesElement.Element(XName.Get("MaxDataDiskCount", "http://schemas.microsoft.com/windowsazure"));
                            if (maxDataDiskCountElement != null)
                            {
                                int maxDataDiskCountInstance = int.Parse(maxDataDiskCountElement.Value, CultureInfo.InvariantCulture);
                                roleSizeInstance.MaxDataDiskCount = maxDataDiskCountInstance;
                            }
                            
                            XElement webWorkerResourceDiskSizeInMbElement = roleSizesElement.Element(XName.Get("WebWorkerResourceDiskSizeInMb", "http://schemas.microsoft.com/windowsazure"));
                            if (webWorkerResourceDiskSizeInMbElement != null)
                            {
                                int webWorkerResourceDiskSizeInMbInstance = int.Parse(webWorkerResourceDiskSizeInMbElement.Value, CultureInfo.InvariantCulture);
                                roleSizeInstance.WebWorkerResourceDiskSizeInMb = webWorkerResourceDiskSizeInMbInstance;
                            }
                            
                            XElement virtualMachineResourceDiskSizeInMbElement = roleSizesElement.Element(XName.Get("VirtualMachineResourceDiskSizeInMb", "http://schemas.microsoft.com/windowsazure"));
                            if (virtualMachineResourceDiskSizeInMbElement != null)
                            {
                                int virtualMachineResourceDiskSizeInMbInstance = int.Parse(virtualMachineResourceDiskSizeInMbElement.Value, CultureInfo.InvariantCulture);
                                roleSizeInstance.VirtualMachineResourceDiskSizeInMb = virtualMachineResourceDiskSizeInMbInstance;
                            }
                        }
                    }
                    
                    result.StatusCode = statusCode;
                    if (httpResponse.Headers.Contains("x-ms-request-id"))
                    {
                        result.RequestId = httpResponse.Headers.GetValues("x-ms-request-id").FirstOrDefault();
                    }
                    
                    if (shouldTrace)
                    {
                        Tracing.Exit(invocationId, result);
                    }
                    return result;
                }
                finally
                {
                    if (httpResponse != null)
                    {
                        httpResponse.Dispose();
                    }
                }
            }
            finally
            {
                if (httpRequest != null)
                {
                    httpRequest.Dispose();
                }
            }
        }
    }
}
