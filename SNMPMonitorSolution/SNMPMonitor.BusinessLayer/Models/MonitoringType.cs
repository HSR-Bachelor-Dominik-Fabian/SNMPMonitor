using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    public class MonitoringType
    {
        private int _monitoringTypeNr;
        private string _description;
        private string _objectID;
        private string _objectIDReplaced;

        public MonitoringType(int monitoringTypeNr, string description, string objectID)
        {
            _monitoringTypeNr = monitoringTypeNr;
            _description = description;
            _objectID = objectID;
            _objectIDReplaced = objectID.Replace('.', '_');
        }

        public int MonitoringTypeNr
        {
            get
            {
                return _monitoringTypeNr;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public string ObjectID
        {
            get
            {
                return _objectID;
            }
        }

        public string ObjectIDReplaced
        {
            get
            {
                return _objectIDReplaced;
            }
        }
    }
}
