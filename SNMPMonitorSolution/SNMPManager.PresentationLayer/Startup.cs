using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SNMPManager.PresentationLayer.Startup))]

namespace SNMPManager.PresentationLayer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SNMPService.Start();
        }
    }
}