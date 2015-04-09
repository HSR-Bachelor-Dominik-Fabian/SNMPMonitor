using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    class MonitoringType
    {
        private int _monitoringTypeNr;
        private String _description;
        private String _objectID;

        public MonitoringType(int monitoringTypeNr, String description, String objectID)
        {
            _monitoringTypeNr = monitoringTypeNr;
            _description = description;
            _objectID = objectID;
        }

        public int MonitoringTypeNr
        {
            get
            {
                return _monitoringTypeNr;
            }
        }

        public String Description
        {
            get
            {
                return _description;
            }
        }

        public String ObjectID
        {
            get
            {
                return _objectID;
            }
        }
    }
}
