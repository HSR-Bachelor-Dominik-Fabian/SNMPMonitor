using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SNMPMonitor.PresentationLayer.Hubs
{
    public class SNMPDataHub : Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SNMPDataHub>();
            context.Clients.All.displayStatus();
        }
    }
}