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
            bool successfull = false;
            try
            {
                string connectionString = Properties.Settings.Default.TestDatabase;
                SNMPController controller = new SNMPController(connectionString);
                controller.GetSNMPDataFromAgents();
                successfull = true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.StackTrace);
                successfull = false;
            }

            Assert.IsTrue(successfull);
        }
    }
}