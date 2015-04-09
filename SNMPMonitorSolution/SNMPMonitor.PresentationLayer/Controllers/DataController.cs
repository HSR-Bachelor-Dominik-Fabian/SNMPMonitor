using Newtonsoft.Json.Linq;
using SNMPMonitor.PresentationLayer.Hubs;
using SNMPMonitor.PresentationLayer.Models;
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
            string param = "{param:[{\"Result\":\"12\"},{\"MonitorTimestamp\":\"2015-04-10 15:02:55.177\"},{\"ObjectID\":\"1.3.6.1.2.1.25.3.3.1.2.8\"},{\"AgentNr\":\"1\"},]}";
            JObject jobject = JObject.Parse(param);
            Models.MonitorDataModel monitor = new Models.MonitorDataModel(jobject);
            Hub.SendSNMPData(monitor);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
        [HttpPost]
        public HttpStatusCodeResult RowInsertedTrigger(string param)
        {
            SNMPDataHub Hub = new SNMPDataHub();
            JObject jobject = JObject.Parse(param);
            Models.MonitorDataModel monitor = new Models.MonitorDataModel(jobject);
            Hub.SendSNMPData(monitor);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public JsonResult HistoryDataForOID(string id, string oid)
        {
            List<MonitorDataModel> models = new List<MonitorDataModel>();

            MonitorDataModel temp = new MonitorDataModel();
            temp.AgentID = 1;
            temp.MonitorTimestamp = DateTime.Now;
            temp.ObjectID = "1.3.6.1.2.1.25.3.3.1.2.8";
            temp.Result = "10";
            models.Add(temp);

            HistoryMonitorDataModel history = new HistoryMonitorDataModel(models);

            return Json(history,JsonRequestBehavior.AllowGet);
        }
    }
}