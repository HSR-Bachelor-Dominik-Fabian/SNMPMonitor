using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMontor.DataLayer
{
    public class AgentModel
    {
        private readonly int _agentNr;
        private readonly String _name;
        private readonly String _iPAddress;
        private readonly int _typeNr;
        private readonly int _port;

        public AgentModel(int agentNr, String name, String iPAddress, int typeNr, int port)
        {
            _agentNr = agentNr;
            _name = name;
            _iPAddress = iPAddress;
            _typeNr = typeNr;
            _port = port;
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

    }
}
