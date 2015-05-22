using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNMPMonitor.BusinessLayer;
using System.Collections.Generic;

namespace SNMPMonitor.BusinessLayer.Tests
{
    [TestClass]
    public class SNMPControllerTests
    {
        SNMPController controller;
        Agent agent = new Agent("sinv-56075.edu.hsr.ch", "152.96.56.75", new Type(1, "Server"), 40001);

        [TestInitialize]
        public void TestSetup()
        {
            controller = new SNMPController(Properties.Settings.Default.TestDatabase);

            List<Agent> agents = controller.GetAgents();

            if (agents.Count == 0)
            {
                controller.AddAgentToDatabase(agent, false, false);
            }
        }
        
        [TestMethod]
        public void TestGetMonitoringTypesForAgent()
        {
            
            List<MonitoringType> monitoringTypesExpected = new List<MonitoringType>();
            monitoringTypesExpected.Add(new MonitoringType(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpected.Add(new MonitoringType(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpected.Add(new MonitoringType(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            monitoringTypesExpected.Add(new MonitoringType(11, "cpuUsage", "1.3.6.1.2.1.25.3.3"));
            monitoringTypesExpected.Add(new MonitoringType(12, "hrDiskStorageTable", "1.3.6.1.2.1.25.3.2"));

            int agentNr = 1;
            List<MonitoringType> monitoringTypesActual = controller.GetMonitoringTypesForAgent(agentNr);

            for (int i = 0; i < monitoringTypesActual.Count; i++)
            {
                Assert.AreEqual(monitoringTypesExpected[i].MonitoringTypeNr, monitoringTypesActual[i].MonitoringTypeNr);
                Assert.AreEqual(monitoringTypesExpected[i].Description, monitoringTypesActual[i].Description);
                Assert.AreEqual(monitoringTypesExpected[i].ObjectID, monitoringTypesActual[i].ObjectID);
            }
        }

        [TestMethod]
        public void GetHistoryOfOIDForAgentTest()
        {
            List<MonitorData> list = controller.GetHistoryOfOIDForAgent(1, "1.3.6.1.2.1.1.5.0", 10);
            Assert.IsTrue(list.Count <= 10);
        }

        [TestMethod]
        public void TestGetAgents()
        {
            //List<Agent> agentList = controller.GetAgents();
            //Assert.AreEqual(1, agentList.Count);
            List<Agent> agents = controller.GetAgents();

            if (agents.Count > 0)
            {
                Agent testAgent = agents[0];
                Assert.AreEqual(1, testAgent.AgentNr);
                Assert.AreEqual("sinv-56075.edu.hsr.ch", testAgent.Name);
                Assert.AreEqual("152.96.56.75", testAgent.IPAddress);
                Type testAgentType = testAgent.Type;
                Assert.AreEqual(1, testAgentType.TypeNr);
                Assert.AreEqual(40001, testAgent.Port);
                Assert.AreEqual(1, testAgent.Status);
                Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.1.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"Hardware: Intel64 Family 6 Model 62 Stepping 4 AT/AT COMPATIBLE - Software: Windows Version 6.3 (Build 9600 Multiprocessor Free)\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysDesc);
                Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.5.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"sinv-56075\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysName);
            }

            Assert.AreEqual(1, agents.Count);
        }

        [TestMethod]
        public void TestGetAgent()
        {
            List<Agent> agents = controller.GetAgents();
            int agentNr;
            Agent agent;
            if (agents.Count > 0)
            {
                agentNr = agents[agents.Count - 1].AgentNr;
                agent = controller.GetAgent(agentNr);

                if (agent != null)
                {
                    Agent testAgent = agent;
                    Assert.AreEqual(1, testAgent.AgentNr);
                    Assert.AreEqual("sinv-56075.edu.hsr.ch", testAgent.Name);
                    Assert.AreEqual("152.96.56.75", testAgent.IPAddress);
                    Type testAgentType = testAgent.Type;
                    Assert.AreEqual(1, testAgentType.TypeNr);
                    Assert.AreEqual(40001, testAgent.Port);
                    Assert.AreEqual(1, testAgent.Status);
                    Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.1.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"Hardware: Intel64 Family 6 Model 62 Stepping 4 AT/AT COMPATIBLE - Software: Windows Version 6.3 (Build 9600 Multiprocessor Free)\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysDesc);
                    Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.5.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"sinv-56075\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), testAgent.SysName);
                }
                else
                {
                    Assert.IsFalse(true);
                }
            }
            else
            {
                Assert.IsFalse(true);
            }
        }
        
        [TestMethod]
        public void TestGetTypes()
        {
            List<Type> typesActual = controller.GetTypes();

            List<Type> typesExpected = new List<Type>() { new Type(1, "Server"), new Type(2, "Switch") };

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
        public void TestAddAndDeleteAgent()
        {
            List<Agent> agentsBeforeAdd = controller.GetAgents();
            Agent newAgent = new Agent("Test-Server", "10.10.10.10", new Type(1, "Server"), 161);

            controller.AddAgentToDatabase(newAgent, false, false);

            List<Agent> agentsAfterAdd = controller.GetAgents();

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
                Assert.AreEqual(agentsBeforeAdd[i].SysDesc, agentsAfterAdd[i].SysDesc);
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
            controller.DeleteAgent(agentsAfterAdd[i].AgentNr);

            List<Agent> agentsAfterDelete = controller.GetAgents();

            Assert.AreEqual(agentsAfterDelete.Count, agentsBeforeAdd.Count);
        }
        
        [TestMethod]
        public void TestUpdateAgent()
        {
            Agent newAgent = new Agent("Test-Server", "10.10.10.10", new Type(1, "Server"), 161);

            // Add new Agent to Database
            controller.AddAgentToDatabase(newAgent, false, false);

            // Get AgentNr for new added Agent
            List<Agent> agentsAfterAdd = controller.GetAgents();
            int agentNr = agentsAfterAdd[agentsAfterAdd.Count - 1].AgentNr;

            // Check MonitoringType for inserted Agent
            List<MonitoringType> monitoringTypesExpectedBeforeUpdate = new List<MonitoringType>();
            monitoringTypesExpectedBeforeUpdate.Add(new MonitoringType(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpectedBeforeUpdate.Add(new MonitoringType(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpectedBeforeUpdate.Add(new MonitoringType(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));

            List<MonitoringType> monitoringTypesActualBeforeUpdate = controller.GetMonitoringTypesForAgent(agentNr);
            for (int i = 0; i < monitoringTypesActualBeforeUpdate.Count; i++)
            {
                Assert.AreEqual(monitoringTypesExpectedBeforeUpdate[i].MonitoringTypeNr, monitoringTypesActualBeforeUpdate[i].MonitoringTypeNr);
                Assert.AreEqual(monitoringTypesExpectedBeforeUpdate[i].Description, monitoringTypesActualBeforeUpdate[i].Description);
                Assert.AreEqual(monitoringTypesExpectedBeforeUpdate[i].ObjectID, monitoringTypesActualBeforeUpdate[i].ObjectID);
            }

            // Update new Agent
            controller.UpdateAgentInDatabase(agentsAfterAdd[agentsAfterAdd.Count - 1], true, true);

            // Check MonitoringType for updated Agent
            List<MonitoringType> monitoringTypesExpectedAfterUpdate = new List<MonitoringType>();
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringType(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringType(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringType(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringType(11, "cpuUsage", "1.3.6.1.2.1.25.3.3"));
            monitoringTypesExpectedAfterUpdate.Add(new MonitoringType(12, "hrDiskStorageTable", "1.3.6.1.2.1.25.3.2"));

            List<MonitoringType> monitoringTypesActualAfterUpdate = controller.GetMonitoringTypesForAgent(agentNr);
            for (int i = 0; i < monitoringTypesActualAfterUpdate.Count; i++)
            {
                Assert.AreEqual(monitoringTypesExpectedAfterUpdate[i].MonitoringTypeNr, monitoringTypesActualAfterUpdate[i].MonitoringTypeNr);
                Assert.AreEqual(monitoringTypesExpectedAfterUpdate[i].Description, monitoringTypesActualAfterUpdate[i].Description);
                Assert.AreEqual(monitoringTypesExpectedAfterUpdate[i].ObjectID, monitoringTypesActualAfterUpdate[i].ObjectID);
            }

            // Delete new Agent
            controller.DeleteAgent(agentNr);
        }
        
        [TestMethod]
        public void TestGetMonitoringSummary()
        {
            List<KeyValuePair<Agent, List<MonitoringType>>> summaryExpected = new List<KeyValuePair<Agent, List<MonitoringType>>>();
            
            Agent agentExpected1 = agent;
            List<MonitoringType> listExpected1 = new List<MonitoringType>();
            listExpected1.Add(new MonitoringType(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            listExpected1.Add(new MonitoringType(9, "sysName", "1.3.6.1.2.1.1.5"));
            listExpected1.Add(new MonitoringType(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            listExpected1.Add(new MonitoringType(11, "cpuUsage", "1.3.6.1.2.1.25.3.3"));
            listExpected1.Add(new MonitoringType(12, "hrDiskStorageTable", "1.3.6.1.2.1.25.3.2"));

            Agent agentExpected2 = new Agent("Test-Server", "10.10.10.10", new Type(1, "Server"), 161);
            List<MonitoringType> listExpected2 = new List<MonitoringType>();
            listExpected2.Add(new MonitoringType(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            listExpected2.Add(new MonitoringType(9, "sysName", "1.3.6.1.2.1.1.5"));
            listExpected2.Add(new MonitoringType(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));

            summaryExpected.Add(new KeyValuePair<Agent, List<MonitoringType>>(agentExpected1, listExpected1));
            summaryExpected.Add(new KeyValuePair<Agent, List<MonitoringType>>(agentExpected2, listExpected2));

            controller.AddAgentToDatabase(agentExpected2, false, false);

            List<KeyValuePair<Agent, List<MonitoringType>>> summaryActual = controller.GetMonitoringSummary();

            Assert.AreEqual(summaryExpected.Count, summaryActual.Count);

            for (int i = 0; i < summaryActual.Count; i++)
            {
                //Test Key (Agent)
                Assert.AreEqual(summaryExpected[i].Key.IPAddress, summaryActual[i].Key.IPAddress);
                Assert.AreEqual(summaryExpected[i].Key.Name, summaryActual[i].Key.Name);
                Assert.AreEqual(summaryExpected[i].Key.Port, summaryActual[i].Key.Port);
                Assert.AreEqual(summaryExpected[i].Key.Status, summaryActual[i].Key.Status);
                Assert.AreEqual(summaryExpected[i].Key.Type.TypeNr, summaryActual[i].Key.Type.TypeNr);
                
                //Test Value (List<MonitoringType>)
                for (int j = 0; j < summaryActual[i].Value.Count; j++)
                {
                    Assert.AreEqual(summaryExpected[i].Value[j].MonitoringTypeNr, summaryActual[i].Value[j].MonitoringTypeNr);
                    Assert.AreEqual(summaryExpected[i].Value[j].Description, summaryActual[i].Value[j].Description);
                    Assert.AreEqual(summaryExpected[i].Value[j].ObjectID, summaryActual[i].Value[j].ObjectID);
                }
            }

            //Delete added agent for testcase
            List<Agent> agents = controller.GetAgents();
            int newAddedAgentNr = agents[agents.Count - 1].AgentNr;
            controller.DeleteAgent(newAddedAgentNr);

        }

        [TestMethod]
        public void TestGetMonitoringSummaryForAgent()
        {
            Agent agentExpected = agent;
            List<MonitoringType> monitoringTypesExpected = new List<MonitoringType>();
            monitoringTypesExpected.Add(new MonitoringType(1, "sysDesc", "1.3.6.1.2.1.1.1"));
            monitoringTypesExpected.Add(new MonitoringType(9, "sysName", "1.3.6.1.2.1.1.5"));
            monitoringTypesExpected.Add(new MonitoringType(10, "sysUptime", "1.3.6.1.2.1.25.1.1"));
            monitoringTypesExpected.Add(new MonitoringType(11, "cpuUsage", "1.3.6.1.2.1.25.3.3"));
            monitoringTypesExpected.Add(new MonitoringType(12, "hrDiskStorageTable", "1.3.6.1.2.1.25.3.2"));
            KeyValuePair<Agent, List<MonitoringType>> summaryExpected = new KeyValuePair<Agent,List<MonitoringType>>(agentExpected, monitoringTypesExpected);

            int agentNr = 1;
            KeyValuePair<Agent, List<MonitoringType>> summaryActual = controller.GetMonitorSummaryForAgent(agentNr);

            // Test Agent Key
            Assert.AreEqual(1, summaryActual.Key.AgentNr);
            Assert.AreEqual("sinv-56075.edu.hsr.ch", summaryActual.Key.Name);
            Assert.AreEqual("152.96.56.75", summaryActual.Key.IPAddress);
            Assert.AreEqual(1, summaryActual.Key.Type.TypeNr);
            Assert.AreEqual(40001, summaryActual.Key.Port);
            Assert.AreEqual(1, summaryActual.Key.Status);
            Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.1.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"Hardware: Intel64 Family 6 Model 62 Stepping 4 AT/AT COMPATIBLE - Software: Windows Version 6.3 (Build 9600 Multiprocessor Free)\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), summaryActual.Key.SysDesc);
            Assert.AreEqual("{\n  \"Results\": [\n    {\n      \"OID\": \"1.3.6.1.2.1.1.5.0\",\n      \"Type\": \"OctetString\",\n      \"Value\": \"sinv-56075\"\n    }\n  ]\n}".Replace("\n", Environment.NewLine), summaryActual.Key.SysName);

            // Test Size of List<MonitoringType>
            Assert.AreEqual(summaryExpected.Value.Count, summaryActual.Value.Count);

            for (int i = 0; i < summaryActual.Value.Count; i++)
            {
                Assert.AreEqual(summaryExpected.Value[i].MonitoringTypeNr, summaryActual.Value[i].MonitoringTypeNr);
                Assert.AreEqual(summaryExpected.Value[i].Description, summaryActual.Value[i].Description);
                Assert.AreEqual(summaryExpected.Value[i].ObjectID, summaryActual.Value[i].ObjectID);
            }
        }
    }
}
