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
using System.Linq;

namespace Microsoft.WindowsAzure.Management.Compute.Models
{
    /// <summary>
    /// The compute capabilities.
    /// </summary>
    public partial class ComputeCapabilities
    {
        private IList<string> _virtualMachinesRoleSizes;
        
        /// <summary>
        /// Optional. Role sizes support for IaaS deployments.
        /// </summary>
        public IList<string> VirtualMachinesRoleSizes
        {
            get { return this._virtualMachinesRoleSizes; }
            set { this._virtualMachinesRoleSizes = value; }
        }
        
        private IList<string> _webWorkerRoleSizes;
        
        /// <summary>
        /// Optional. Role sizes support for PaaS deployments.
        /// </summary>
        public IList<string> WebWorkerRoleSizes
        {
            get { return this._webWorkerRoleSizes; }
            set { this._webWorkerRoleSizes = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the ComputeCapabilities class.
        /// </summary>
        public ComputeCapabilities()
        {
            this._virtualMachinesRoleSizes = new List<string>();
            this._webWorkerRoleSizes = new List<string>();
        }
    }
}
