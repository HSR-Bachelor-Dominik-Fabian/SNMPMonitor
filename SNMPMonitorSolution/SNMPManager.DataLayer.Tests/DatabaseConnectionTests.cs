using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNMPManager.DataLayer;
using System.Collections.Generic;

namespace SNMPManager.DataLayer.Tests
{
    [TestClass]
    public class DatabaseConnectionTests
    {
        private DatabaseSettings databaseSettingsTest;
        DatabaseConnection databaseConnection;

        [TestInitialize]
        public void TestSetup()
        {
            databaseSettingsTest = new DatabaseSettings("152.96.56.75", 40003, "ManagerTest", "HSR-00228866", "SNMPMonitorTest");
            databaseConnection = new DatabaseConnection(databaseSettingsTest);

            List<AgentModel> agents = databaseConnection.GetAgentsFromDatabase();
            
            if(agents.Count == 0) {
                AgentModel agent = new AgentModel(1, "sinv-56075.edu.hsr.ch", "152.96.56.75", 1, 40003);
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
            DatabaseConnection databaseConnection = new DatabaseConnection(databaseSettingsTest);


            int actual = databaseConnection.GetMonitoringTypesForAgentFromDatabase(1).Count;

            int expected = 4;

            Assert.AreEqual(expected, actual);
        }
    }
}
