﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Networks.Tests
{
    using System;
    using System.Linq;
    using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
    using SubResource = Microsoft.Azure.Management.Network.Models.SubResource;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Azure.Management.Network;
    using Microsoft.Azure.Management.Network.Models;
    using Microsoft.Azure.Management.Resources;
    using Microsoft.Azure.Management.Resources.Models;
    using Microsoft.Azure.Test;
    using Networks.Tests.Helpers;
    using ResourceGroups.Tests;
    using Xunit;

    public class ExpressRouteTests
    {
        public const string MS_PrimaryPrefix = "199.168.200.0/30";
        public const string MS_SecondaryPrefix = "199.168.202.0/30";
        public const string MS_PeerASN = "1000";
        public const string MS_PublicPrefix = "12.2.3.4/30";

        public const string Circuit_Provider = "bvtazureixp01";
        public const string Circuit_Location = "boydton 1 dc";
        public const string Circuit_BW = "200";
        public const string MS_VlanId = "400";

        public const string Filter_Commmunity = "12076:5010";
        public const string Filter_Access = "allow";
        public const string Filter_Type = "Community";

        public const string Peering_Microsoft = "MicrosoftPeering";
        [Fact]
        public void BGPCommunityApiTest()
        {
            var handler1 = new RecordedDelegatingHandler { StatusCodeToReturn = HttpStatusCode.OK };
            var handler2 = new RecordedDelegatingHandler { StatusCodeToReturn = HttpStatusCode.OK };

            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var resourcesClient = ResourcesManagementTestUtilities.GetResourceManagementClientWithHandler(context, handler1);
                var networkManagementClient = NetworkManagementTestUtilities.GetNetworkManagementClientWithHandler(context, handler2);
                var location = NetworkManagementTestUtilities.GetResourceLocation(resourcesClient, "Microsoft.Network/routefilters");

                var communities = networkManagementClient.BgpServiceCommunities.List();

                Assert.NotNull(communities);
                Assert.True(communities.First().BgpCommunities.First().IsAuthorizedToUse);
            }
        }

        [Fact]
        public void RouteFilterApiTest()
        {
            var handler1 = new RecordedDelegatingHandler { StatusCodeToReturn = HttpStatusCode.OK };
            var handler2 = new RecordedDelegatingHandler { StatusCodeToReturn = HttpStatusCode.OK };

            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var resourcesClient = ResourcesManagementTestUtilities.GetResourceManagementClientWithHandler(context, handler1);
                var networkManagementClient = NetworkManagementTestUtilities.GetNetworkManagementClientWithHandler(context, handler2);
                var location = NetworkManagementTestUtilities.GetResourceLocation(resourcesClient, "Microsoft.Network/routefilters");

                string resourceGroupName = TestUtilities.GenerateName("csmrg");
                resourcesClient.ResourceGroups.CreateOrUpdate(
                    resourceGroupName,
                    new ResourceGroup
                    {
                        Location = location
                    });

                // Create route filter 
                string filterName = "filter";
                string ruleName = "rule";

                var filter = TestHelper.CreateDefaultRouteFilter(resourceGroupName,
                    filterName, location, networkManagementClient);

                Assert.Equal(filter.Name, filterName);

                // Update filter with rules
                var rule = TestHelper.CreateDefaultRouteFilterRule(resourceGroupName,
                    filterName, ruleName, location, networkManagementClient);

                // Delete resource group
                resourcesClient.ResourceGroups.Delete(resourceGroupName);
            }
        }

        [Fact]
        public void ExpressRouteMicrosoftPeeringApiTest()
        {
            var handler1 = new RecordedDelegatingHandler { StatusCodeToReturn = HttpStatusCode.OK };
            var handler2 = new RecordedDelegatingHandler { StatusCodeToReturn = HttpStatusCode.OK };

            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var resourcesClient = ResourcesManagementTestUtilities.GetResourceManagementClientWithHandler(context, handler1);
                var networkManagementClient = NetworkManagementTestUtilities.GetNetworkManagementClientWithHandler(context, handler2);
                var location = "westus" ;

                string resourceGroupName = TestUtilities.GenerateName("csmrg");
                resourcesClient.ResourceGroups.CreateOrUpdate(
                    resourceGroupName,
                    new ResourceGroup
                    {
                        Location = location
                    });

                string circuitName = "circuit";                           

                var circuit = TestHelper.CreateDefaultExpressRouteCircuit(resourceGroupName,
                    circuitName, location, networkManagementClient);

                Assert.Equal(circuit.Name, circuitName);
                Assert.Equal(circuit.ServiceProviderProperties.BandwidthInMbps, Convert.ToInt32(Circuit_BW));

                circuit = TestHelper.UpdateDefaultExpressRouteCircuitWithMicrosoftPeering(resourceGroupName,
                    circuitName, networkManagementClient);

                Assert.Equal(circuit.Name, circuitName);
                Assert.Equal(circuit.ServiceProviderProperties.BandwidthInMbps, Convert.ToInt32(Circuit_BW));
                Assert.NotNull(circuit.Peerings);

                resourcesClient.ResourceGroups.Delete(resourceGroupName);
            }
        }
    }
}