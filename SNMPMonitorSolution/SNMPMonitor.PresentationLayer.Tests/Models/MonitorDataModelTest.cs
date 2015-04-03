using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SNMPMonitor.PresentationLayer.Models;

namespace SNMPMonitor.PresentationLayer.Tests.Models
{
    /// <summary>
    /// Summary description for MonitorDataModelTest
    /// </summary>
    [TestClass]
    public class MonitorDataModelTest
    {
        [TestMethod]
        public void TestMonitorDataModelJSONConstructor()
        {
            string param = "{param:[{\"Result\":\"Test123\"},{\"MonitorTimestamp\":\"2015-04-01 15:02:55.177\"},{\"ObjectID\":\"TEstOBject\"},{\"AgentNr\":\"123\"},]}";
            DateTime time = DateTime.Parse("2015-04-01 15:02:55.177");
            JObject jobject = JObject.Parse(param);
            MonitorDataModel monitor = new MonitorDataModel(jobject);
            Assert.AreEqual("Test123", monitor.Result);
            Assert.AreEqual(time, monitor.MonitorTimestamp);
            Assert.AreEqual("TEstOBject", monitor.ObjectID);
            Assert.AreEqual(123, monitor.AgentID);
        }
    }
}
