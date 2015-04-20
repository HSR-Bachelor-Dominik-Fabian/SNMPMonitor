using SNMPMonitor.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNMPMonitor.PresentationLayer.Models
{
    public class HistoryMonitorDataModel
    {
        private List<MonitorDataModel> _history;
        public List<MonitorDataModel> History
        {
            get
            {
                return this._history;
            }
            set
            {
                this._history = value;
            }
        }

        public HistoryMonitorDataModel(List<MonitorData> history)
        {
            this._history = new List<MonitorDataModel>();
            foreach (MonitorData monitor in history)
            {
                MonitorDataModel model = new MonitorDataModel(monitor);
                this._history.Add(model);
            }
        }
    }
}