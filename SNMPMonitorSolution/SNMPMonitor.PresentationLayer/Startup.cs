using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SNMPMonitor.PresentationLayer.Startup))]

namespace SNMPMonitor.PresentationLayer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            //SNMPMonitor.BusinessLayer.Startup.Configuration(Properties.Settings.Default.SNMPMonitorConnectionString);
        }
    }
}
