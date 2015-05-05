using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNMPMonitor.DataLayer;
using System.Collections.Generic;

namespace SNMPMonitor.DataLayer.Tests
{
    [TestClass]
    public class DatabaseConnectionTests
    {

        DatabaseConnectionMonitor databaseConnection;

        [TestInitialize]
        public void TestSetup()
        {
            databaseConnection = new DatabaseConnectionMonitor(Properties.Settings.Default.TestDatabase);

            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();

            if (agents.Count == 0)
            {
                AgentDataModel agent = new AgentDataModel(1, "sinv-56075.edu.hsr.ch", "152.96.56.75", new TypeDataModel(1, "Server"), 40001, 1, "Test-Client", "sinv-56075", "");
                databaseConnection.AddAgentToDatabase(agent);
            }
        }

        [TestMethod]
        public void getHistoryOfOIDForAgent()
        {
            List<MonitorDataDataModel> monitorDataList = databaseConnection.GetHistoryOfOIDForAgent(1, "1.3.6.1.2.1.1.5.0", 10);

            int expected = 10;

            Assert.IsTrue(monitorDataList.Count <= expected);
        }
    }
}
