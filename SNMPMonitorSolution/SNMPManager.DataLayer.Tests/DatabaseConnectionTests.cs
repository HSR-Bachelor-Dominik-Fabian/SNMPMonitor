using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNMPManager.DataLayer;
using System.Collections.Generic;

namespace SNMPManager.DataLayer.Tests
{
    [TestClass]
    public class DatabaseConnectionTests
    {
        DatabaseConnectionManager databaseConnection;

        [TestInitialize]
        public void TestSetup()
        {
            databaseConnection = new DatabaseConnectionManager(Properties.Settings.Default.TestDatabase);

            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();
            
            if(agents.Count == 0) {
                AgentDataModel agent = new AgentDataModel(1, "sinv-56075.edu.hsr.ch", "152.96.56.75", 1, 40003, 1, "Test-Client", "sinv-56075", "");
                databaseConnection.AddAgentToDatabase(agent);
            }
        }

        [TestMethod]
        public void TestDatabaseConnectionGetAgents()
        {

            int actual = databaseConnection.GetAgentsFromDatabase().Count;

            int expected = 1;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDatabaseConnectionGetMonitoringTypes()
        {
            int actual = databaseConnection.GetMonitoringTypesForAgentForCheckFromDatabase(1).Count;

            int expected = 4;

            Assert.AreEqual(expected, actual);
        }
    }
}
