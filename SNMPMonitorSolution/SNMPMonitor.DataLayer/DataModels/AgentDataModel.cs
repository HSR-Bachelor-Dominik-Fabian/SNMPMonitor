using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.DataLayer
{
    public class AgentDataModel
    {
        private readonly int _agentNr;
        private readonly String _name;
        private readonly String _iPAddress;
        private readonly int _typeNr;
        private readonly int _port;
        private readonly int _status;

        public AgentDataModel(int agentNr, String name, String iPAddress, int typeNr, int port, int status)
        {
            _agentNr = agentNr;
            _name = name;
            _iPAddress = iPAddress;
            _typeNr = typeNr;
            _port = port;
            _status = status;
        }

        public int AgentNr
        {
            get
            {
                return _agentNr;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
        }

        public String IPAddress
        {
            get
            {
                return _iPAddress;
            }
        }

        public int TypeNr
        {
            get
            {
                return _typeNr;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
        }

        public int Status
        {
            get
            {
                return _status;
            }
        }

    }
}
