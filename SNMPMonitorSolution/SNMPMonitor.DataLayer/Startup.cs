using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.DataLayer
{
    public class Startup
    {
        public static void Configuration(string connectionString)
        {
            SQLDependency.DependencyConfig config = new SQLDependency.DependencyConfig(connectionString);
            //config.startupDependency();
        }
    }
}
