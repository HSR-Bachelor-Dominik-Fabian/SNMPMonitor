using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class AgentModel
    {
        private int _agtNr;
        private String _name;
        private String _iPAddress;
        private int _typeNr;
        private int _port;

        public AgentModel(int AgtNr, String Name, String IPAddress, int TypeNr, int Port)
        {
            _agtNr = AgtNr;
            _name = Name;
            _iPAddress = IPAddress;
            _typeNr = TypeNr;
            _port = Port;
        }

        public int AgtNr
        {
            get
            {
                return _agtNr;
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
