using Newtonsoft.Json.Linq;
using SNMPMonitor.PresentationLayer.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
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
        [HttpPost]
        public HttpStatusCodeResult RowInsertedTrigger(string param)
        {
            SNMPDataHub Hub = new SNMPDataHub();
            JObject jobject = JObject.Parse(param);
            Hub.SendSNMPData(jobject);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}