using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class DatabaseSettings
    {
        private readonly String _location;
        private readonly int _port;
        private readonly String _user;
        private readonly String _password;
        private readonly String _databaseName;

        public DatabaseSettings(String location, int port, String user, String password, String databaseName)
        {
            _location = location;
            _port = port;
            _user = user;
            _password = password;
            _databaseName = databaseName;
        }

        public String Location
        {
            get
            {
                return _location;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
        }

        public String User
        {
            get
            {
                return _user;
            }
        }

        public String Password
        {
            get
            {
                return _password;
            }
        }

        public String DatabaseName
        {
            get
            {
                return _databaseName;
            }
        }
    }
}
