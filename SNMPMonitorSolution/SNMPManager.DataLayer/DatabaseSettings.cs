using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class DatabaseSettings
    {
        private readonly string _connectionString;

        public DatabaseSettings(string connectionString)
        {
            _connectionString = connectionString;
        }

        public String ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
