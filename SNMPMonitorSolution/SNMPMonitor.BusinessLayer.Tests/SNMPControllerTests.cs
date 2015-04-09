using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNMPMonitor.BusinessLayer;

namespace SNMPMonitor.BusinessLayer.Tests
{
    [TestClass]
    public class SNMPControllerTests
    {
        [TestInitialize]
        public void TestSetup()
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.TestDatabase);
        }
        
        [TestMethod]
        public void GetMonitoringTypesFromAgentTest()
        {
            Assert.IsTrue(true);
        }
    }
}
