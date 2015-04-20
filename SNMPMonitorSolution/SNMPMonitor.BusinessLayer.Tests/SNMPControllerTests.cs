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

        [TestInitialize]
        public void TestSetup()
        {
            controller = new SNMPController(Properties.Settings.Default.TestDatabase);
        }
        
        [TestMethod]
        public void GetMonitoringTypesFromAgentTest()
        {
            // In Testdatenbank sind 4 MonitoringTypes für Agent 1 hinterlegt
            List<MonitoringType> list = controller.GetMonitoringTypesForAgent(1);
            Assert.AreEqual(4, list.Count);
        }

        [TestMethod]
        public void GetHistoryOfOIDForAgentTest()
        {
            List<MonitorData> list = controller.GetHistoryOfOIDForAgent(1, "1.3.6.1.2.1.1.5.0");
            Assert.IsTrue(list.Count <= 10);
        }

        [TestMethod]
        public void GetMonitoringSummaryTest()
        {
            // 1 Agent in Testdatenbank vorhanden
            List<KeyValuePair<Agent, List<MonitoringType>>> list = controller.GetMonitoringSummary();
            Assert.AreEqual(1, list.Count);
            
            // 4 MonitoringTypes für Agent 1 hinterlegt
            KeyValuePair<Agent, List<MonitoringType>> kvp = list[0];
            List<MonitoringType> monitoringTypeList = kvp.Value;
            Assert.AreEqual(4, monitoringTypeList.Count);
        }
        
    }
}
