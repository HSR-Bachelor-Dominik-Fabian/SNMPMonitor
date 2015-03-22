using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNMPMonitor.PresentationLayer
{
    public class SNMPData
    {
        public int ID { get; set; }
        public String IPAddress { get; set; }
        public String OID { get; set; }
        public String Result { get; set; }
    }
}