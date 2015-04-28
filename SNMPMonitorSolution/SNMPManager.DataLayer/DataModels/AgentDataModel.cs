using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class AgentDataModel
    {
        private readonly int _agentNr;
        private readonly string _name;
        private readonly string _iPAddress;
        private readonly int _typeNr;
        private readonly int _port;
        private readonly int _status;
        private readonly string _sysDesc;
        private readonly string _sysName;

        public AgentDataModel(int agentNr, string name, string iPAddress, int typeNr, int port, int status, string sysDesc, string sysName)
        {
            _agentNr = agentNr;
            _name = name;
            _iPAddress = iPAddress;
            _typeNr = typeNr;
            _port = port;
            _status = status;
            _sysDesc = sysDesc;
            _sysName = sysName;
        }

        public string SysName
        {
            get
            {
                return _sysName;
            }
        }

        public string SysDescription
        {
            get
            {
                return _sysDesc;
            }
        }

        public int AgentNr
        {
            get
            {
                return _agentNr;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string IPAddress
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
