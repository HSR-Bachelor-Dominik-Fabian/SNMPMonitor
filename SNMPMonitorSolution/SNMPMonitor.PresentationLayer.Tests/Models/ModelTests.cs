using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SNMPMonitor.PresentationLayer.Models;
using SNMPMonitor.BusinessLayer;

namespace SNMPMonitor.PresentationLayer.Tests.Models
{
    /// <summary>
    /// Summary description for MonitorDataModelTest
    /// </summary>
    [TestClass]
    public class ModelTests
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
        public void TestMonitorDataModelGetSet()
        {
            DateTime Now = DateTime.Now;
            MonitorDataModel model = new MonitorDataModel();
            model.AgentID = 12;
            model.ObjectID = "1.132.3";
            model.MonitorTimestamp = Now;
            model.Result = "Result";

            Assert.AreEqual(12, model.AgentID);
            Assert.AreEqual("1.132.3", model.ObjectID);
            Assert.AreEqual(Now, model.MonitorTimestamp);
            Assert.AreEqual("Result", model.Result);
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
        public void TestEventModelBusinessConstructor()
        {
            DateTime Now = DateTime.Now;
            Event NewEvent = new Event(12,"category","Normal",Now,123,"Message","Stacktrace");
            EventModel eventModel = new EventModel(NewEvent);
            Assert.AreEqual(12, eventModel.EventNr);
            Assert.AreEqual("category", eventModel.ExceptionType);
            Assert.AreEqual("Normal", eventModel.Category);
            Assert.AreEqual(Now, eventModel.EventTimestamp);
            Assert.AreEqual(123, eventModel.HResult);
            Assert.AreEqual("Message", eventModel.Message);
            Assert.AreEqual("Stacktrace", eventModel.Stacktrace);
        }

        [TestMethod]
        public void TestAgentModelJSONConstructor()
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

        [TestMethod]
        [ExpectedException(typeof(FormatException), "The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'Name','<value>'},{'IPAddress','<value>'},{'Status','<value>'},{'Port','<value>'},{'sysDesc','<value>'},{'sysName','<value>'},{'sysUptime','<value>'}]}")]
        public void TestAgentModelThrowsAgentNrJSONConstructor()
        {
            string param = "{param:[{\"AgentNr\":\"error\"}, {\"Name\":\"Test-Agent\"}, {\"IPAddress\":\"10.10.10.10\"}, {\"Status\":\"1\"}, {\"Port\":\"161\"}, {\"sysDesc\":\"Test-SysDesc\"}, {\"sysName\":\"Test-SysName\"}, {\"sysUptime\":\"Test-SysUptime\"}]}";
            JObject jobject = JObject.Parse(param);
            AgentModel agent = new AgentModel(jobject);
            
        }
        [TestMethod]
        [ExpectedException(typeof(FormatException), "The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'Name','<value>'},{'IPAddress','<value>'},{'Status','<value>'},{'Port','<value>'},{'sysDesc','<value>'},{'sysName','<value>'},{'sysUptime','<value>'}]}")]
        public void TestAgentModelThrowsStatusJSONConstructor()
        {
            string param = "{param:[{\"AgentNr\":\"1\"}, {\"Name\":\"Test-Agent\"}, {\"IPAddress\":\"10.10.10.10\"}, {\"Status\":\"error\"}, {\"Port\":\"161\"}, {\"sysDesc\":\"Test-SysDesc\"}, {\"sysName\":\"Test-SysName\"}, {\"sysUptime\":\"Test-SysUptime\"}]}";
            JObject jobject = JObject.Parse(param);
            AgentModel agent = new AgentModel(jobject);

        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'Name','<value>'},{'IPAddress','<value>'},{'Status','<value>'},{'Port','<value>'},{'sysDesc','<value>'},{'sysName','<value>'},{'sysUptime','<value>'}]}")]
        public void TestAgentModelThrowsPortJSONConstructor()
        {
            string param = "{param:[{\"AgentNr\":\"1\"}, {\"Name\":\"Test-Agent\"}, {\"IPAddress\":\"10.10.10.10\"}, {\"Status\":\"1\"}, {\"Port\":\"error\"}, {\"sysDesc\":\"Test-SysDesc\"}, {\"sysName\":\"Test-SysName\"}, {\"sysUptime\":\"Test-SysUptime\"}]}";
            JObject jobject = JObject.Parse(param);
            AgentModel agent = new AgentModel(jobject);

        }

        [TestMethod]
        public void TestAgentModelEmptyConstructor()
        {
            AgentModel model = new AgentModel();
            Assert.AreEqual(0, model.AgentNr);
            Assert.AreEqual(string.Empty, model.Name);
            Assert.AreEqual(string.Empty,model.IPAddress);
            Assert.AreEqual(0, model.Status);
            Assert.AreEqual(0, model.Port);
            Assert.AreEqual(string.Empty, model.SysDesc);
            Assert.AreEqual(string.Empty,model.SysName);
            Assert.AreEqual(string.Empty, model.SysUptime);
        }

        [TestMethod]
        public void TestAgentModelBusinessConstructor()
        {
            Agent business = new Agent("Name","IP",new BusinessLayer.Type(1,"Type"),123);
            AgentModel model = new AgentModel(business);
            Assert.AreEqual(0, model.AgentNr);
            Assert.AreEqual("Name", model.Name);
            Assert.AreEqual("IP", model.IPAddress);
            Assert.AreEqual(1, model.Status);
            Assert.AreEqual(123, model.Port);
            Assert.AreEqual(string.Empty, model.SysDesc);
            Assert.AreEqual(string.Empty, model.SysName);
            Assert.AreEqual(string.Empty, model.SysUptime);
        }

        [TestMethod]
        public void TestAgentModelGetSet()
        {
            Agent business = new Agent("Name", "IP", new BusinessLayer.Type(1, "Type"), 123);
            AgentModel model = new AgentModel();
            model.Name = "Name";
            model.IPAddress = "IP";
            model.Status = 1;
            model.Port = 123;
            model.SysDesc = "SysDesc";
            model.SysName = "SysName";
            model.SysUptime = "123d 123h 1m";
            model.AgentNr = 12;

            Assert.AreEqual(12, model.AgentNr);
            Assert.AreEqual("Name", model.Name);
            Assert.AreEqual("IP", model.IPAddress);
            Assert.AreEqual(1, model.Status);
            Assert.AreEqual(123, model.Port);
            Assert.AreEqual("SysDesc", model.SysDesc);
            Assert.AreEqual("SysName", model.SysName);
            Assert.AreEqual("123d 123h 1m", model.SysUptime);
        }

        [TestMethod]
        public void TestHistoryDataModelConstructor()
        {
            DateTime Now = DateTime.Now;
            BusinessLayer.MonitorData monitor1 = new BusinessLayer.MonitorData(Now, "Result", 1, "1.2.3.4.5");
            BusinessLayer.MonitorData monitor2 = new BusinessLayer.MonitorData(Now, "Result", 2, "1.2.3.4.5.6");
            List<BusinessLayer.MonitorData> List = new List<BusinessLayer.MonitorData>();
            List.Add(monitor1);
            List.Add(monitor2);

            HistoryMonitorDataModel model = new HistoryMonitorDataModel(List);

            Assert.AreEqual(monitor2.Timestamp, model.History[1].MonitorTimestamp);
            Assert.AreEqual(monitor2.ObjectID, model.History[1].ObjectID);
            Assert.AreEqual(monitor2.AgentNr, model.History[1].AgentID);
            Assert.AreEqual(monitor2.Result, model.History[1].Result);
        }
    }
}
