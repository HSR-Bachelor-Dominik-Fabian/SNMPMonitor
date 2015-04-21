using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class MonitoringTypeDataModel
    {
        private readonly int _monitoringTypeNr;
        private readonly String _description;
        private readonly String _objectID;

        public MonitoringTypeDataModel(int monitoringTypeNr, String description, String objectID)
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
