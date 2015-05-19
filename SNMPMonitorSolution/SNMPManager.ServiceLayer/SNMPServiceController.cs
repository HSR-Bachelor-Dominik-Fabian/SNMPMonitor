using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.ServiceLayer
{
    public partial class SNMPServiceController : ServiceBase
    {
        public SNMPServiceController()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SNMPServiceTimer.Start();
        }

        protected override void OnStop()
        {
            SNMPServiceTimer.Stop();
        }
    }
}
