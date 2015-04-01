using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SNMPManager.BusinessLayer.Tests
{
    [TestClass]
    public class SNMPControllerTest
    {
        [TestMethod]
        public void TestSNMPController()
        {
            string connectionString = Properties.Settings.Default.TestDatabase;
            SNMPController controller = new SNMPController(connectionString);
            controller.SaveSNMPDataFromAgentsToDatabase();
        }
    }
}