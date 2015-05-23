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
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
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
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
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
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, e);
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
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
                output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            return output;
        }

        [HttpGet]
        public CustomJsonResult HistoryDataForOID(string id, string oid, int count)
        {
            int agentId;
            HistoryMonitorDataModel history = null;
            try
            {
                SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
                
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
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
            return new CustomJsonResult { Data = history, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public CustomJsonResult GetMonitorSummary()
        {
            List<KeyValuePair<Agent, List<MonitoringType>>> monitorSummary = null;
            try
            {
                SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
                monitorSummary = controller.GetMonitoringSummary();
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
            
            return new CustomJsonResult { Data = monitorSummary, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public CustomJsonResult GetMonitorSummaryForAgent(int id)
        {
            KeyValuePair<Agent, List<MonitoringType>> monitorSummary = null;
            try
            {
                SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
                monitorSummary = controller.GetMonitorSummaryForAgent(id);
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
           
            return new CustomJsonResult { Data = monitorSummary, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public CustomJsonResult GetAllEvents()
        {
            List<Event> eventSummary = null;
            try
            {
                SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
                eventSummary = controller.GetAllEvents();
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
            
            return new CustomJsonResult { Data = eventSummary , JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [HttpPost]
        public HttpStatusCodeResult AddAgentToMonitor(string inputAgentName, string inputIpAddress, string inputPortNr, string typeName, bool cpuCheck, bool discCheck)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            try
            {
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
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
            
            return output;
        }

        [HttpGet]
        public CustomJsonResult GetAgents()
        {
            List<Agent> agents = null;
            try
            {
                SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabaseConnectionString);
                agents = controller.GetAgents();
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
            
            return new CustomJsonResult { Data = agents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public HttpStatusCodeResult UpdateAgentInMonitor(int inputAgentNr, string inputAgentName, string inputIpAddress, string inputPortNr, string typeName, bool cpuCheck, bool discCheck)
        {
            HttpStatusCodeResult output = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            try
            {
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
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
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