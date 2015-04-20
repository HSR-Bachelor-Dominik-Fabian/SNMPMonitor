using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.DataLayer
{
    public class MonitoringTypeDataModel
    {
        private readonly int _monitoringTypeNr;
        private readonly String _description;
        private readonly String _objectID;
        private readonly bool _isLongTimeCheck;

        public MonitoringTypeDataModel(int monitoringTypeNr, String description, String objectID, bool IsLongTimeCheck)
        {
            _monitoringTypeNr = monitoringTypeNr;
            _description = description;
            _objectID = objectID;
            _isLongTimeCheck = IsLongTimeCheck;
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

        public bool IsLongTimeCheck
        {
            get
            {
                return _isLongTimeCheck;
            }
        }
    }
}
