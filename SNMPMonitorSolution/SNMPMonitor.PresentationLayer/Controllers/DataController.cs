using SNMPMonitor.PresentationLayer.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNMPMonitor.PresentationLayer.Controllers
{
    public class DataController : Controller
    {
        public HttpStatusCodeResult RowInsertedTrigger()
        {
            SNMPDataHub Hub = new SNMPDataHub();
            Hub.SendSNMPData();
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}