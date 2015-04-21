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
        private static Timer _shortTimer = new Timer();
        private static SNMPController controller = new SNMPController(Properties.Settings.Default.ProdDatabase);

        public static void Start()
        {
            _shortTimer.Elapsed += new ElapsedEventHandler(OnShortTimedEvent);
            _shortTimer.Interval = 5000;
            _shortTimer.Enabled = true;
        }

        public static void Stop()
        {
            _shortTimer.Enabled = false;
        }

        public static Boolean IsRunning()
        {
            return _shortTimer.Enabled;
        }

        private static void OnShortTimedEvent(object source, ElapsedEventArgs e)
        {
            controller.GetSNMPDataFromAgents();
        }
    }
}