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
        AgentDataModel agent = new AgentDataModel("sinv-56075.edu.hsr.ch", "152.96.56.75", new TypeDataModel(1, "Server"), 40001);

        [TestInitialize]
        public void TestSetup()
        {
            databaseConnection = new DatabaseConnectionMonitor(Properties.Settings.Default.TestDatabase);

            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();

            if (agents.Count == 0)
            {
                databaseConnection.AddAgentToDatabase(agent, false, false);
            }
        }

        [TestMethod]
        public void getHistoryOfOIDForAgent()
        {
            List<MonitorDataDataModel> monitorDataList = databaseConnection.GetHistoryOfOIDForAgent(1, "1.3.6.1.2.1.1.5.0", 10);

            int expected = 10;

            Assert.IsTrue(monitorDataList.Count <= expected);
        }
       
        [TestMethod]
        public void TestGetTypesFromDatabase()
        {
            List<TypeDataModel> typesActual = databaseConnection.GetTypesFromDatabase();

            List<TypeDataModel> typesExpected = new List<TypeDataModel>() { new TypeDataModel(1, "Server"), new TypeDataModel(2, "Switch") };

            if (typesActual.Count == 2)
            {
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
        public void TestGetMonitoringTypesForAgentFromDatabase() 
        {
            List<MonitoringTypeDataModel> monitoringTypesExpected = new List<MonitoringTypeDataModel>();
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(11, "cpuUsage", "1.3.6.1.2.1.25.3.3"));
            monitoringTypesExpected.Add(new MonitoringTypeDataModel(12, "hrDiskStorageTable", "1.3.6.1.2.1.25.3.2"));

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
        public void TestGetAgentsFromDatabase()
        {
            List<AgentDataModel> agents = databaseConnection.GetAgentsFromDatabase();

            if (agents.Count > 0)
            {
                AgentDataModel testAgent = agents[0];
                Assert.AreEqual(1, testAgent.AgentNr);
                Assert.AreEqual("sinv-56075.edu.hsr.ch", testAgent.Name);
                Assert.AreEqual("152.96.56.75", testAgent.IPAddress);
                TypeDataModel testAgentType = testAgent.Type;
                Assert.AreEqual(1, testAgentType.TypeNr);
                Assert.AreEqual(40001, testAgent.Port);
                Assert.AreEqual(1, testAgent.Status);
                Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.1.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"Hardware: Intel64 Family 6 Model 62 Stepping 4 AT/AT COMPATIBLE - Software: Windows Version 6.3 (Build 9600 Multiprocessor Free)\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysDescription);
                Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.5.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"sinv-56075\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysName);
            }

            Assert.AreEqual(1, agents.Count);
        }

        [TestMethod]
        public void TestAddAndDeleteAgentFromDatabase()
        {
            List<AgentDataModel> agentsBeforeAdd = databaseConnection.GetAgentsFromDatabase();
            AgentDataModel newAgent = new AgentDataModel("Test-Server", "10.10.10.10", new TypeDataModel(1, "Server"), 161);

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
                Assert.AreEqual(agentsBeforeAdd[i].Type.TypeNr, agentsAfterAdd[i].Type.TypeNr);
                Assert.AreEqual(agentsBeforeAdd[i].Port, agentsAfterAdd[i].Port);
                Assert.AreEqual(agentsBeforeAdd[i].Status, agentsAfterAdd[i].Status);
                Assert.AreEqual(agentsBeforeAdd[i].SysDescription, agentsAfterAdd[i].SysDescription);
                Assert.AreEqual(agentsBeforeAdd[i].SysName, agentsAfterAdd[i].SysName);
                Assert.AreEqual(agentsBeforeAdd[i].SysUptime, agentsAfterAdd[i].SysUptime);
            }

            // Check new Agent
            Assert.AreEqual(newAgent.Name, agentsAfterAdd[i].Name);
            Assert.AreEqual(newAgent.IPAddress, agentsAfterAdd[i].IPAddress);
            Assert.AreEqual(newAgent.Type.TypeNr, agentsAfterAdd[i].Type.TypeNr);
            Assert.AreEqual(newAgent.Port, agentsAfterAdd[i].Port);
            Assert.AreEqual(newAgent.Status, agentsAfterAdd[i].Status);

            // Delete new Agent
            databaseConnection.DeleteAgentInDatabase(agentsAfterAdd[i].AgentNr);

            List<AgentDataModel> agentsAfterDelete = databaseConnection.GetAgentsFromDatabase();

            Assert.AreEqual(agentsAfterDelete.Count, agentsBeforeAdd.Count);
        }
        
        [TestMethod]
        public void TestAddAndGetEventFromDatabase()
        {
            List<EventDataModel> eventsBeforeAdd = databaseConnection.GetAllEventsFromDatabase();

            databaseConnection.AddEventToDatabase("FormatException", "HIGH", Convert.ToDateTime("07.05.2015 10:10:10"), "2", "Nice try", "Stacktrace");

            List<EventDataModel> eventsAfterAdd = databaseConnection.GetAllEventsFromDatabase();

            Assert.AreEqual(eventsBeforeAdd.Count + 1, eventsAfterAdd.Count);
            
            int lastIndex = 0;
            
            // Check added Event
            Assert.AreEqual("FormatException", eventsAfterAdd[lastIndex].ExceptionType);
            Assert.AreEqual("HIGH", eventsAfterAdd[lastIndex].Category);
            Assert.AreEqual("07.05.2015 10:10:10", eventsAfterAdd[lastIndex].EventTimestamp.ToString());
            Assert.AreEqual(2, eventsAfterAdd[lastIndex].HResult);
            Assert.AreEqual("Nice try", eventsAfterAdd[lastIndex].Message);
            Assert.AreEqual("Stacktrace", eventsAfterAdd[lastIndex].Stacktrace);
            
        }

        [TestMethod]
        public void TestGetAgentFromDatabase()
        {
            AgentDataModel agent = databaseConnection.GetAgentFromDatabase(1);

            if (agent != null)
            {
                AgentDataModel testAgent = agent;
                Assert.AreEqual(1, testAgent.AgentNr);
                Assert.AreEqual("sinv-56075.edu.hsr.ch", testAgent.Name);
                Assert.AreEqual("152.96.56.75", testAgent.IPAddress);
                TypeDataModel testAgentType = testAgent.Type;
                Assert.AreEqual(1, testAgentType.TypeNr);
                Assert.AreEqual(40001, testAgent.Port);
                Assert.AreEqual(1, testAgent.Status);
                Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.1.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"Hardware: Intel64 Family 6 Model 62 Stepping 4 AT/AT COMPATIBLE - Software: Windows Version 6.3 (Build 9600 Multiprocessor Free)\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysDescription);
                Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.5.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"sinv-56075\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysName);
            }
            else
            {
                Assert.IsFalse(true);
            }
        }
             
        [TestMethod]
        public void TestUpdateAgentInDatabase()
        {
            AgentDataModel newAgent = new AgentDataModel("Test-Server", "10.10.10.10", new TypeDataModel(1, "Server"), 161);
            
            // Add new Agent to Database
            databaseConnection.AddAgentToDatabase(newAgent, false, false);

            // Get AgentNr for new added Agent
            List<AgentDataModel> agentsAfterAdd = databaseConnection.GetAgentsFromDatabase();
            int agentNr = agentsAfterAdd[agentsAfterAdd.Count - 1].AgentNr;

            // Check MonitoringType for inserted Agent
            List<MonitoringTypeDataModel> monitoringTypesExpectedBeforeUpdate = new List<MonitoringTypeDataModel>();
            monitoringTypesExpectedBeforeUpdate.Add(new MonitoringTypeDataModel(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpectedBeforeUpdate.Add(new MonitoringTypeDataModel(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpectedBeforeUpdate.Add(new MonitoringTypeDataModel(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            
            List<MonitoringTypeDataModel> monitoringTypesActualBeforeUpdate = databaseConnection.GetMonitoringTypesForAgentFromDatabase(agentNr);
            for (int i = 0; i < monitoringTypesActualBeforeUpdate.Count; i++)
            {
                Assert.AreEqual(monitoringTypesExpectedBeforeUpdate[i].MonitoringTypeNr, monitoringTypesActualBeforeUpdate[i].MonitoringTypeNr);
                Assert.AreEqual(monitoringTypesExpectedBeforeUpdate[i].Description, monitoringTypesActualBeforeUpdate[i].Description);
                Assert.AreEqual(monitoringTypesExpectedBeforeUpdate[i].ObjectID, monitoringTypesActualBeforeUpdate[i].ObjectID);
            }

            // Update new Agent
            databaseConnection.UpdateAgentInDatabase(agentsAfterAdd[agentsAfterAdd.Count - 1], true, true);

            // Check MonitoringType for updated Agent
            List<MonitoringTypeDataModel> monitoringTypesExpectedAfterUpdate = new List<MonitoringTypeDataModel>();
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringTypeDataModel(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringTypeDataModel(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringTypeDataModel(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringTypeDataModel(11, "cpuUsage", "1.3.6.1.2.1.25.3.3"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringTypeDataModel(12, "hrDiskStorageTable", "1.3.6.1.2.1.25.3.2"));

            List<MonitoringTypeDataModel> monitoringTypesActualAfterUpdate = databaseConnection.GetMonitoringTypesForAgentFromDatabase(agentNr);
            for (int i = 0; i < monitoringTypesActualAfterUpdate.Count; i++)
            {
                Assert.AreEqual(monitoringTypesExpectedAfterUpdate[i].MonitoringTypeNr, monitoringTypesActualAfterUpdate[i].MonitoringTypeNr);
                Assert.AreEqual(monitoringTypesExpectedAfterUpdate[i].Description, monitoringTypesActualAfterUpdate[i].Description);
                Assert.AreEqual(monitoringTypesExpectedAfterUpdate[i].ObjectID, monitoringTypesActualAfterUpdate[i].ObjectID);
            }

            // Delete new Agent
            databaseConnection.DeleteAgentInDatabase(agentNr);
        }
       
    }
}
