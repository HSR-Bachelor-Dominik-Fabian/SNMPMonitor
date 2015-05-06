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

        [TestMethod]
        public void TestEventModelJSONConstructor()
        {
            string param = "{param:[{\"EventNr\":\"1\"},{\"ExceptionType\":\"SnmpSharpNet.SnmpException\"},{\"Category\":\"High\"},{\"EventTimestamp\":\"2015-05-06 23:01:22.517\"},{\"HResult\":\"-2146233088\"},{\"Message\":\"Test-Message\"},{\"Stacktrace\":\"Test-Stacktrace\"}]}";
            JObject jobject = JObject.Parse(param);
            EventModel eventModel = new EventModel(jobject);
            Assert.AreEqual(1, eventModel.EventNr);
            Assert.AreEqual("SnmpSharpNet.SnmpException", eventModel.ExceptionType);
            Assert.AreEqual("High", eventModel.Category);
            Assert.AreEqual(DateTime.Parse("2015-05-06 23:01:22.517"), eventModel.EventTimestamp);
            Assert.AreEqual(-2146233088, eventModel.HResult);
            Assert.AreEqual("Test-Message", eventModel.Message);
            Assert.AreEqual("Test-Stacktrace", eventModel.Stacktrace);
        }

        [TestMethod]
        public void TestAgentModelJSONContructor()
        {
            string param = "{param:[{\"AgentNr\":\"1\"}, {\"Name\":\"Test-Agent\"}, {\"IPAddress\":\"10.10.10.10\"}, {\"Status\":\"1\"}, {\"Port\":\"161\"}, {\"sysDesc\":\"Test-SysDesc\"}, {\"sysName\":\"Test-SysName\"}, {\"sysUptime\":\"Test-SysUptime\"}]}";
            JObject jobject = JObject.Parse(param);
            AgentModel agent = new AgentModel(jobject);
            Assert.AreEqual(1, agent.AgentNr);
            Assert.AreEqual("Test-Agent", agent.Name);
            Assert.AreEqual("10.10.10.10", agent.IPAddress);
            Assert.AreEqual(1, agent.Status);
            Assert.AreEqual(161, agent.Port);
            Assert.AreEqual("Test-SysDesc", agent.SysDesc);
            Assert.AreEqual("Test-SysName", agent.SysName);
            Assert.AreEqual("Test-SysUptime", agent.SysUptime);
        }
    }
}
