using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Linq;
using SNMPMonitor.PresentationLayer.Models;

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
            context.Clients.Group(".1.3.2.14.23").receiveData(rnd.Next(100));
        }
        [Obsolete]
        public void SendSNMPData(string dataToShow)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            context.Clients.Group(".1.3.2.14.23").receiveData(dataToShow);
        }
        public void SendSNMPData(JObject dataToShow)
        {
            MonitorDataModel model = new MonitorDataModel();
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            context.Clients.Group(".1.3.2.14.23").receiveData(dataToShow);
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