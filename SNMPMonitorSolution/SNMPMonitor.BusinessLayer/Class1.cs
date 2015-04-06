using SNMPMonitor.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    public class Class1
    {
        public static void Main()
        {
            string conStr = Properties.Settings.Default.ProdDatabase;
            DatabaseConnectionMonitor connection = new DatabaseConnectionMonitor(conStr);

            connection.AddAgentToDatabaseForDemo("Test", "152.96.56.75", 40001);
        }
    }
}
