using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Linq;
using SNMPMonitor.PresentationLayer.Models;
using System.Web.Helpers;
using Newtonsoft.Json;
using SNMPMonitor.BusinessLayer;

namespace SNMPMonitor.PresentationLayer.Hubs
{
    [HubName("snmpDataHub")]
    public class SNMPDataHub : Hub
    {
        public void SendSNMPData(MonitorDataModel dataToShow)
        {
            try
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();

                context.Clients.Group("Agent_General").receiveData(JObject.FromObject(dataToShow, this.GetSerializer()));
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
            
        }

        public void SendNewEvent(EventModel eventModel)
        {
            try
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();

                context.Clients.Group("Agent_General").receiveNewEvent(JObject.FromObject(eventModel, this.GetSerializer()));
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
        }

        public void SendUpdatedAgent(AgentModel agent)
        {
            try
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
                context.Clients.Group("Agent_General").receiveUpdatedAgentWithValue(JObject.FromObject(agent, this.GetSerializer()));
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
        }

        public void SendInsertDelete()
        {
            try
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
                context.Clients.Group("Agent_General").receiveInsertDelete();
            }
            catch (Exception exc)
            {
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.Normal, exc);
            }
        }

        public Task JoinDataGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveDataGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        private JsonSerializer GetSerializer() 
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            return JsonSerializer.Create(serializerSettings);
        }
    }
}