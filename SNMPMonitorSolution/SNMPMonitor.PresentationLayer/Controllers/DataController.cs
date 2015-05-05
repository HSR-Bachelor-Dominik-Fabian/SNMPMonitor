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
        public HttpStatusCodeResult RowInsertedTrigger(string id  = "1", string result = "12")
        {            
            SNMPDataHub Hub = new SNMPDataHub();
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string param = "{param:[{\"Result\":\""+result+"\"},{\"MonitorTimestamp\":\""+ date +"\"},{\"ObjectID\":\"1.3.6.1.2.1.25.3.3.1.2.8\"},{\"AgentNr\":\""+id+"\"},]}";
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
        [HttpPost]
        public HttpStatusCodeResult AgentUpdatedTrigger(string param)
        {
            try
            {
                SNMPDataHub Hub = new SNMPDataHub();
                JObject jobject = JObject.Parse(param);
                Models.AgentModel agent = new Models.AgentModel(jobject);
                Hub.SendUpdatedAgent(agent);
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Low, exc);
            }
            
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public JsonResult HistoryDataForOID(string id, string oid, int count)
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            int agentId;
            HistoryMonitorDataModel history = null;
            if (int.TryParse(id, out agentId))
            {
                List<MonitorData> businessLayerHistory = controller.GetHistoryOfOIDForAgent(agentId, oid, count);
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

        [HttpGet]
        public JsonResult GetAllEvents()
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            List<Event> monitorSummary = controller.GetAllEvents();
            return Json(monitorSummary, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public JsonResult GetAgents()
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            List<Agent> agents = controller.GetAgents();
            JsonResult test = Json(agents, JsonRequestBehavior.AllowGet);
            return test;
        }
    }
}