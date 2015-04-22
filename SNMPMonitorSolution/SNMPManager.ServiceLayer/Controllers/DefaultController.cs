using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SNMPManager.BusinessLayer;
using System.Timers;
using System.Web;

namespace SNMPManager.ServiceLayer.Controllers
{
    public class DefaultController : ApiController
    {

        [HttpGet, ActionName("startSNMPService")]
        public String startSNMPService()
        {
            SNMPService.Start();
            return "SNMP Service started";
        }

        [HttpGet, ActionName("stopSNMPService")]
        public String stopSNMPService()
        {
            if(SNMPService.IsRunning()) {
                SNMPService.Stop();
            }
            return "SNMP Service stopped";
        }
    }
}
