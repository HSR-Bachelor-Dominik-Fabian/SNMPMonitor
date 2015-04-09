using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    public class Startup
    {
        public static void Configuration(string connectionString)
        {
            SNMPMonitor.DataLayer.Startup.Configuration(connectionString);
        }
    }
}
