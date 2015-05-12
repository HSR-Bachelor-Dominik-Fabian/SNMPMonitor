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
        AgentDataModel agent = new AgentDataModel(1, "sinv-56075.edu.hsr.ch", "152.96.56.75", 1, 40001, 1, "", "", "");

        [TestInitialize]
        public void TestSetup()
        {
            databaseConnection = new DatabaseConnectionManager(Properties.Settings.Default.TestDatabase);

            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();
            
            if(agents.Count == 0) {
                databaseConnection.AddAgentToDatabase(agent, false, false);
            }
        }

        [TestMethod]
        public void TestGetAgents()
        {

            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();

            if (agents.Count > 0)
            {
                AgentDataModel testAgent = agents[0];
                Assert.AreEqual(1, testAgent.AgentNr);
                Assert.AreEqual("sinv-56075.edu.hsr.ch", testAgent.Name);
                Assert.AreEqual("152.96.56.75", testAgent.IPAddress);
                Assert.AreEqual(1, testAgent.TypeNr);
                Assert.AreEqual(40001, testAgent.Port);
                Assert.AreEqual(1, testAgent.Status);
            }

            Assert.AreEqual(1, agents.Count);
        }

        [TestMethod]
        public void TestGetMonitoringTypesForAgentForCheckSameResultMultipleCalls()
        {
            int agentNr = 1;
            List<MonitoringTypeDataModel> monitoringTypes1 = databaseConnection.GetMonitoringTypesForAgentForCheckFromDatabase(agentNr);

            List<MonitoringTypeDataModel> monitoringTypes2 = databaseConnection.GetMonitoringTypesForAgentForCheckFromDatabase(agentNr);

            if(monitoringTypes1.Count == monitoringTypes2.Count) {
                for (int i = 0; i < monitoringTypes1.Count; i++)
                {
                    Assert.AreEqual(monitoringTypes1[i].MonitoringTypeNr, monitoringTypes2[i].MonitoringTypeNr);
                    Assert.AreEqual(monitoringTypes1[i].ObjectID, monitoringTypes2[i].ObjectID);
                    Assert.AreEqual(monitoringTypes1[i].Description, monitoringTypes2[i].Description);
                }
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void TestGetMonitoringTypesForAgent()
        {
            List<MonitoringTypeDataModel> monitoringTypesExpected = new List<MonitoringTypeDataModel>();
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(4, "hrMemorySize", "1.3.6.1.2.1.25.2.2"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(11, "cpuUsage", "1.3.6.1.2.1.25.3.3"));

            int agentNr = 1;
            List<MonitoringTypeDataModel> monitoringTypesActual = databaseConnection.GetMonitoringTypesForAgentFromDatabase(agentNr);

            for (int i = 0; i < monitoringTypesActual.Count; i++)
            {
                Assert.AreEqual(monitoringTypesExpected[i].MonitoringTypeNr, monitoringTypesActual[i].MonitoringTypeNr);
                Assert.AreEqual(monitoringTypesExpected[i].Description, monitoringTypesActual[i].Description);
                Assert.AreEqual(monitoringTypesExpected[i].ObjectID, monitoringTypesActual[i].ObjectID);
            }
        }

        [TestMethod]
        public void TestGetTypes()
        {
            List<TypeDataModel> typesActual = databaseConnection.GetTypesFromDatabase();

            List<TypeDataModel> typesExpected = new List<TypeDataModel>() {new TypeDataModel(1, "Server"), new TypeDataModel(2, "Switch")};

            if(typesActual.Count == 2) {
                for (int i = 0; i < typesActual.Count; i++)
                {
                    Assert.AreEqual(typesActual[i].TypeNr, typesExpected[i].TypeNr);
                    Assert.AreEqual(typesActual[i].Name, typesExpected[i].Name);
                }
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void TestAddAndDeleteAgent()
        {
            List<AgentDataModel> agentsBeforeAdd = databaseConnection.GetAgentsFromDatabase();
            AgentDataModel newAgent = new AgentDataModel(1, "Test-Server", "10.10.10.10", 1, 161, 1, "", "", "");

            databaseConnection.AddAgentToDatabase(newAgent, false, false);

            List<AgentDataModel> agentsAfterAdd = databaseConnection.GetAgentsFromDatabase();

            Assert.AreEqual(agentsBeforeAdd.Count + 1, agentsAfterAdd.Count);

            // Check old Agents
            int i;
            for (i = 0; i < agentsBeforeAdd.Count; i++)
            {
                Assert.AreEqual(agentsBeforeAdd[i].AgentNr, agentsAfterAdd[i].AgentNr);
                Assert.AreEqual(agentsBeforeAdd[i].Name, agentsAfterAdd[i].Name);
                Assert.AreEqual(agentsBeforeAdd[i].IPAddress, agentsAfterAdd[i].IPAddress);
                Assert.AreEqual(agentsBeforeAdd[i].TypeNr, agentsAfterAdd[i].TypeNr);
                Assert.AreEqual(agentsBeforeAdd[i].Port, agentsAfterAdd[i].Port);
                Assert.AreEqual(agentsBeforeAdd[i].Status, agentsAfterAdd[i].Status);
                Assert.AreEqual(agentsBeforeAdd[i].SysDescription, agentsAfterAdd[i].SysDescription);
                Assert.AreEqual(agentsBeforeAdd[i].SysName, agentsAfterAdd[i].SysName);
                Assert.AreEqual(agentsBeforeAdd[i].SysUptime, agentsAfterAdd[i].SysUptime);
            }
            
            // Check new Agent
            Assert.AreEqual(newAgent.Name, agentsAfterAdd[i].Name);
            Assert.AreEqual(newAgent.IPAddress, agentsAfterAdd[i].IPAddress);
            Assert.AreEqual(newAgent.TypeNr, agentsAfterAdd[i].TypeNr);
            Assert.AreEqual(newAgent.Port, agentsAfterAdd[i].Port);
            Assert.AreEqual(newAgent.Status, agentsAfterAdd[i].Status);

            // Delete new Agent
            databaseConnection.DeleteAgentInDatabase(agentsAfterAdd[i].AgentNr);

            List<AgentDataModel> agentsAfterDelete = databaseConnection.GetAgentsFromDatabase();

            Assert.AreEqual(agentsAfterDelete.Count, agentsBeforeAdd.Count);
        }

        [TestMethod]
        public void TestStatusUpdate()
        {
            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();
            int oldStatus = 0;
            foreach(AgentDataModel agent in agents) {
                if(agent.AgentNr == 1) {
                    oldStatus = agent.Status;
                }
            }

            Assert.AreEqual(1, oldStatus);

            databaseConnection.UpdateStatusOfAgent(1, 3);

            List<AgentDataModel> agentsUpdated = databaseConnection.GetAgentsFromDatabase();
            int newStatus = 0;
            foreach (AgentDataModel agent in agentsUpdated)
            {
                if (agent.AgentNr == 1)
                {
                    newStatus = agent.Status;
                }
            }

            Assert.AreEqual(3, newStatus);

            // Clean up
            databaseConnection.UpdateStatusOfAgent(1, oldStatus);
        }

        [TestMethod]
        public void TestAddMonitorDataToDatabase()
        {
            // Get Agents before adding data for clean up
            string sysDescOld = string.Empty;
            string sysNameOld = string.Empty;
            string sysUptimeOld = string.Empty;
            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();
            foreach(AgentDataModel agent in agents) {
                if(agent.AgentNr == 1) {
                    sysDescOld = agent.SysDescription;
                    sysNameOld = agent.SysName;
                    sysUptimeOld = agent.SysUptime;
                }
            }

            // Set new data
            string sysDescNew = "Test-SysDesc: windows 2008 Server R2";
            string sysNameNew = "Test-SysName: testserver";
            string sysUptimeNew = "Test-SysUptime: 2d 3h 5m";

            List<KeyValuePair<string, string>> monitorData = new List<KeyValuePair<string, string>>();
            monitorData.Add(new KeyValuePair<string, string>("1.3.6.1.2.1.1.1", sysDescNew));
            monitorData.Add(new KeyValuePair<string, string>("1.3.6.1.2.1.1.5", sysNameNew));
            monitorData.Add(new KeyValuePair<string, string>("1.3.6.1.2.1.25.1.1", sysUptimeNew));

            databaseConnection.AddMonitorDataToDatabase(1, monitorData);

            // Check new data
            List<AgentDataModel> agentsNew = databaseConnection.GetAgentsFromDatabase();
            foreach (AgentDataModel agent in agentsNew)
            {
                if (agent.AgentNr == 1)
                {
                    Assert.AreEqual(sysDescNew, agent.SysDescription);
                    Assert.AreEqual(sysNameNew, agent.SysName);
                    Assert.AreEqual(sysUptimeNew, agent.SysUptime);
                }
            }

            //CLean up
            List<KeyValuePair<string, string>> monitorDataOld = new List<KeyValuePair<string, string>>();
            monitorData.Add(new KeyValuePair<string, string>("1.3.6.1.2.1.1.1", sysDescOld));
            monitorData.Add(new KeyValuePair<string, string>("1.3.6.1.2.1.1.5", sysNameOld));
            monitorData.Add(new KeyValuePair<string, string>("1.3.6.1.2.1.25.1.1", sysUptimeOld));

            databaseConnection.AddMonitorDataToDatabase(1, monitorData);
        }


    }
}