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

namespace SNMPMonitor.PresentationLayer.Hubs
{
    [HubName("snmpDataHub")]
    public class SNMPDataHub : Hub
    {
        [Obsolete]
        public void SendSNMPData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            Random rnd = new Random();
            context.Clients.Group("Agent_1").receiveData(rnd.Next(100));
        }
        public void SendSNMPData(MonitorDataModel dataToShow)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            
            context.Clients.Group("Agent_" + dataToShow.AgentID).receiveData(JObject.FromObject(dataToShow));
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