using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    public class MonitorData
    {
        private readonly DateTime _timestamp;
        private readonly string _result;
        private readonly int _agentNr;
        private readonly string _objectID;

        public MonitorData(DateTime timestamp, string result, int agentNr, string objectID)
        {
            _timestamp = timestamp;
            _result = result;
            _agentNr = agentNr;
            _objectID = objectID;
        }

        public DateTime Timestamp
        {
            get
            {
                return _timestamp;
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
        }

        public int AgentNr
        {
            get
            {
                return _agentNr;
            }
        }

        public string ObjectID
        {
            get
            {
                return _objectID;
            }
        }
    }
}
