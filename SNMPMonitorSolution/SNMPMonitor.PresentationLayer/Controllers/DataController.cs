using Newtonsoft.Json.Linq;
using SNMPMonitor.BusinessLayer;
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
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            int agentId;
            HistoryMonitorDataModel history = null;
            if (int.TryParse(id, out agentId))
            {
                List<MonitorData> businessLayerHistory = controller.GetHistoryOfOIDForAgent(agentId, oid);
                businessLayerHistory.Sort((item1, item2) => item1.Timestamp.CompareTo(item2.Timestamp));
                history = new HistoryMonitorDataModel(businessLayerHistory);
            }
            else
            {
                //TODO: Return Error
            }
            return Json(history,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMonitorSummary()
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            List<KeyValuePair<Agent, List<MonitoringType>>> monitorSummary = controller.GetMonitoringSummary();
            return Json(monitorSummary,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCodeResult AddAgentToMonitor(string inputAgentName, string inputIpAddress, string inputPortNr)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            int portNr;
            if(int.TryParse(inputPortNr, out portNr)){
                controller.AddAgentToDatabaseForDemo(inputAgentName, inputIpAddress, portNr);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }

            return output;
        }
    }
}