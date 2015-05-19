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
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            
            context.Clients.Group("Agent_" + dataToShow.AgentID).receiveData(JObject.FromObject(dataToShow));
        }

        public void SendNewEvent(EventModel eventModel)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();

            context.Clients.Group("Agent_General").receiveNewEvent(JObject.FromObject(eventModel));
        }

        public void SendUpdatedAgent(AgentModel agent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            context.Clients.Group("Agent_General").receiveUpdatedAgentWithValue(JObject.FromObject(agent));
        }

        public void SendInsertDelete()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            context.Clients.Group("Agent_General").receiveInsertDelete();
        }

        public Task JoinDataGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveDataGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
    }
}