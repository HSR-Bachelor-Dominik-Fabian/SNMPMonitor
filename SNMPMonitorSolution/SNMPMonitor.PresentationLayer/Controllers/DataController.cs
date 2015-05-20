using Newtonsoft.Json;
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
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            try
            {
                SNMPDataHub Hub = new SNMPDataHub();
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string param = "{param:[{\"Result\":\"" + result + "\"},{\"MonitorTimestamp\":\"" + date + "\"},{\"ObjectID\":\"1.3.6.1.2.1.25.3.3.1.2.8\"},{\"AgentNr\":\"" + id + "\"},]}";
                JObject jobject = JObject.Parse(param);
                Models.MonitorDataModel monitor = new Models.MonitorDataModel(jobject);
                Hub.SendSNMPData(monitor);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (FormatException e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
            }
            return output;
        }

        [HttpPost]
        public HttpStatusCodeResult RowInsertedTrigger(string param)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            try
            {
                SNMPDataHub Hub = new SNMPDataHub();
                JObject jobject = JObject.Parse(param);
                Models.MonitorDataModel monitor = new Models.MonitorDataModel(jobject);
                Hub.SendSNMPData(monitor);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (FormatException e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
            }
            return output;
        }

        [HttpPost]
        public HttpStatusCodeResult NewEventTrigger(string param)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            try
            {
                SNMPDataHub Hub = new SNMPDataHub();
                JObject jobject = JObject.Parse(param);
                Models.EventModel eventModel = new Models.EventModel(jobject);
                Hub.SendNewEvent(eventModel);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (FormatException e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
            }
            return output;
        }

        [HttpPost]
        public HttpStatusCodeResult AgentUpdatedTrigger(string param)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            try
            {
                SNMPDataHub Hub = new SNMPDataHub();
                JObject jobject = JObject.Parse(param);
                Models.AgentModel agent = new Models.AgentModel(jobject);
                Hub.SendUpdatedAgent(agent);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (FormatException e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
            }
            return output;
        }

        [HttpPost]
        public HttpStatusCodeResult InsertDeleteTrigger()
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            try
            {
                SNMPDataHub Hub = new SNMPDataHub();
                Hub.SendInsertDelete();
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
            }
            return output;
        }

        [HttpGet]
        public CustomJsonResult HistoryDataForOID(string id, string oid, int count)
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
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, new FormatException("HistoryDataForOID: id not Integer"));
                //TODO: Return Error
            }
            return new CustomJsonResult { Data = history, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public CustomJsonResult GetMonitorSummary()
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            List<KeyValuePair<Agent, List<MonitoringType>>> monitorSummary = controller.GetMonitoringSummary();
            return new CustomJsonResult { Data = monitorSummary, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public CustomJsonResult GetMonitorSummaryForAgent(int id)
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            KeyValuePair<Agent, List<MonitoringType>> monitorSummary = controller.GetMonitorSummaryForAgent(id);
            return new CustomJsonResult { Data = monitorSummary, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public CustomJsonResult GetAllEvents()
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            List<Event> eventSummary = controller.GetAllEvents();
            return new CustomJsonResult { Data = eventSummary , JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [HttpPost]
        public HttpStatusCodeResult AddAgentToMonitor(string inputAgentName, string inputIpAddress, string inputPortNr, string typeName, bool cpuCheck, bool discCheck)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            int portNr;
            if (int.TryParse(inputPortNr, out portNr))
            {
                List<SNMPMonitor.BusinessLayer.Type> types = controller.GetTypes();
                SNMPMonitor.BusinessLayer.Type type = null;
                foreach (SNMPMonitor.BusinessLayer.Type temp in types)
                {
                    if (temp.Name == typeName)
                    {
                        type = temp;
                    }
                }
                Agent newAgent = new Agent(inputAgentName, inputIpAddress, type, portNr);
                controller.AddAgentToDatabase(newAgent, cpuCheck, discCheck);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            return output;
        }

        [HttpGet]
        public CustomJsonResult GetAgents()
        {
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            List<Agent> agents = controller.GetAgents();
            return new CustomJsonResult { Data = agents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public HttpStatusCodeResult UpdateAgentInMonitor(int inputAgentNr, string inputAgentName, string inputIpAddress, string inputPortNr, string typeName, bool cpuCheck, bool discCheck)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
            int portNr;
            if (int.TryParse(inputPortNr, out portNr))
            {
                List<SNMPMonitor.BusinessLayer.Type> types = controller.GetTypes();
                SNMPMonitor.BusinessLayer.Type type = null;
                foreach (SNMPMonitor.BusinessLayer.Type temp in types)
                {
                    if (temp.Name == typeName)
                    {
                        type = temp;
                    }
                }
                Agent updatedAgent = new Agent(inputAgentNr, inputAgentName, inputIpAddress, type, portNr, 1, "", "", "");
                controller.UpdateAgentInDatabase(updatedAgent, cpuCheck, discCheck);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            return output;
        }

        [HttpPost]
        public HttpStatusCodeResult DeleteAgent(int id)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.Conflict);
            try
            {
                if (id > 0)
                {
                    SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
                    controller.DeleteAgent(id);
                    output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
                }
                else
                {
                    output = new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }

            return output;
        }
    }
}