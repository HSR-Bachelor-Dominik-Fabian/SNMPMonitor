using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    public class Agent
    {
        private readonly int _agentNr;
        private readonly String _name;
        private readonly String _iPAddress;
        private readonly Type _type;
        private readonly int _port;
        private readonly int _status;
        private readonly string _sysDesc;
        private readonly string _sysName;
        private readonly string _sysUptime;

        public Agent(String name, String iPAddress, Type type, int port)
        {
            _agentNr = 0;
            _name = name;
            _iPAddress = iPAddress;
            _type = type;
            _port = port;
            _status = 1;
            _sysDesc = "";
            _sysName = "";
            _sysUptime = "";
        }

        public Agent(int agentNr, String name, String iPAddress, Type type, int port, int status, string sysDesc, string sysName, string sysUptime)
        {
            _agentNr = agentNr;
            _name = name;
            _iPAddress = iPAddress;
            _type = type;
            _port = port;
            _status = status;
            _sysDesc = sysDesc;
            _sysName = sysName;
            _sysUptime = sysUptime;
        }

        public string SysDesc
        {
            get
            {
                return _sysDesc;
            }
        }

        public string SysName
        {
            get
            {
                return _sysName;
            }
        }

        public string SysUptime
        {
            get
            {
                return _sysUptime;
            }
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

        public Type Type
        {
            get
            {
                return _type;
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
