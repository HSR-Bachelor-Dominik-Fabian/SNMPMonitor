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
            string sqlConnectionString = "User id=Manager;" +
                           "Password=HSR-00228866;Data Source=tcp:152.96.56.75,40003;" +
                           "Trusted_Connection=yes;integrated security=False;" +
                           "database=SignalR; " +
                           "connection timeout=30";
            GlobalHost.DependencyResolver.UseSqlServer(sqlConnectionString);

            app.MapSignalR();
        }
    }
}
