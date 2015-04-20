using SNMPManager.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace SNMPManager.PresentationLayer
{
    public static class SNMPService
    {
        private static Timer _timer = new Timer();
        private static SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabase);

        public static void Start()
        {
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Interval = 5000;
            _timer.Enabled = true;
        }

        public static void Stop()
        {
            _timer.Enabled = false;
        }

        public static Boolean IsRunning()
        {
            return _timer.Enabled;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            controller.SaveSNMPDataFromAgentsToDatabase();
        }
    }
}