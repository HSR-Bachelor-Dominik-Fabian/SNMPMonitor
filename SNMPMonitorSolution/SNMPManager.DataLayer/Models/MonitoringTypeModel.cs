using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class MonitoringTypeModel
    {
        private int _moniTypeNr;
        private String _description;
        private String _objectID;

        public MonitoringTypeModel(int MoniTypeNr, String Description, String ObjectID)
        {
            _moniTypeNr = MoniTypeNr;
            _description = Description;
            _objectID = ObjectID;
        }

        public int MoniTypeNr
        {
            get
            {
                return _moniTypeNr;
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
