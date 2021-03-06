﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.Sql;
using Microsoft.Azure.Management.Sql.Models;
using System.Collections.Generic;
using Xunit;

namespace Sql.Tests
{
    public class DatabaseCopyScenarioTests
    {
        [Fact]
        public void TestCopyDatabase()
        {
            string testPrefix = "sqlcrudtest-";
            Dictionary<string, string> tags = new Dictionary<string, string>();
            string suiteName = this.GetType().FullName;

            SqlManagementTestUtilities.RunTestInNewResourceGroup(suiteName, "TestCopyDatabase", testPrefix, (resClient, sqlClient, resourceGroup) =>
            {
                //Create two servers
                var server = SqlManagementTestUtilities.CreateServer(sqlClient, resourceGroup, testPrefix);
                var server2 = SqlManagementTestUtilities.CreateServer(sqlClient, resourceGroup, testPrefix);

                // Create a database with all parameters specified
                // 
                string dbName = SqlManagementTestUtilities.GenerateName(testPrefix);
                var dbInput = new Database()
                {
                    Location = server.Location,
                    Collation = SqlTestConstants.DefaultCollation,
                    Edition = SqlTestConstants.DefaultDatabaseEdition,

                    // Make max size bytes less than default, to ensure that copy follows this parameter
                    MaxSizeBytes = (500 * 1024L * 1024L).ToString(),
                    RequestedServiceObjectiveName = SqlTestConstants.DefaultDatabaseEdition,
                    RequestedServiceObjectiveId = ServiceObjectiveId.Basic,
                    CreateMode = "Default"
                };
                var db = sqlClient.Databases.CreateOrUpdate(resourceGroup.Name, server.Name, dbName, dbInput);
                Assert.NotNull(db);

                // Create a database as copy of the first database
                //
                dbName = SqlManagementTestUtilities.GenerateName(testPrefix);
                var dbInputCopy = new Database()
                {
                    Location = server2.Location,
                    CreateMode = CreateMode.Copy,
                    SourceDatabaseId = db.Id
                };
                var dbCopy = sqlClient.Databases.CreateOrUpdate(resourceGroup.Name, server2.Name, dbName, dbInputCopy);
                SqlManagementTestUtilities.ValidateDatabase(db, dbCopy, dbCopy.Name);
            });
        }
    }
}
