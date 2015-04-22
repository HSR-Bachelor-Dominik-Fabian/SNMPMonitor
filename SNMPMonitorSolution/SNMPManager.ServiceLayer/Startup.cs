using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SNMPManager.ServiceLayer.Startup))]

namespace SNMPManager.ServiceLayer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SNMPService.Start();
        }
    }
}