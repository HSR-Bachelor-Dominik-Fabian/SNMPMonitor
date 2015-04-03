using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNMPManager.DataLayer;
using System.Collections.Generic;

namespace SNMPManager.DataLayer.Tests
{
    [TestClass]
    public class DatabaseConnectionTests
    {
        DatabaseConnection databaseConnection;

        [TestInitialize]
        public void TestSetup()
        {
            databaseConnection = new DatabaseConnection(Properties.Settings.Default.TestDatabase);

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
            int actual = databaseConnection.GetMonitoringTypesForAgentFromDatabase(1).Count;

            int expected = 4;

            Assert.AreEqual(expected, actual);
        }
    }
}
