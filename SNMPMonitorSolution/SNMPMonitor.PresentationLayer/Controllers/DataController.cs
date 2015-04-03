using Newtonsoft.Json.Linq;
using SNMPMonitor.PresentationLayer.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNMPMonitor.PresentationLayer.Controllers
{
    public class DataController : Controller
    {
        public HttpStatusCodeResult RowInsertedTrigger()
        {            
            SNMPDataHub Hub = new SNMPDataHub();
            string param = "{param:[{\"Result\":\"Test123\"},{\"MonitorTimestamp\":\"2015-04-01 15:02:55.177\"},{\"ObjectID\":\"Test123\"},{\"AgentNr\":\"123\"},]}";
            JObject jobject = JObject.Parse(param);
            Models.MonitorDataModel monitor = new Models.MonitorDataModel(jobject);
            Hub.SendSNMPData();
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
        [HttpPost]
        public HttpStatusCodeResult RowInsertedTrigger(string param)
        {
            SNMPDataHub Hub = new SNMPDataHub();
            JObject jobject = JObject.Parse(param);
            Models.MonitorDataModel monitor = new Models.MonitorDataModel(jobject);
            Hub.SendSNMPData(JObject.FromObject(monitor));
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}