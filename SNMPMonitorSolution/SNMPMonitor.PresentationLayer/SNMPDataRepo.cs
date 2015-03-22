using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SNMPMonitor.PresentationLayer
{
    public class SNMPDataRepo
    {
        public IEnumerable<SNMPData> GetData()
        {
            using (SqlConnection _myConnection = new SqlConnection("User id=Manager;" +
               "Password=HSR-00228866;Data Source=tcp:152.96.56.75,40004;" +
               "Trusted_Connection=yes;integrated security=False;" +
               "database=ManagerTestDatabase; " +
               "connection timeout=5")) {

                _myConnection.Open();

                using (SqlCommand command = new SqlCommand(@"SELECT [ID],[IPAdresse],[OID],[Result]
               FROM [dbo].[ManagerTestDatabase]", _myConnection))
                {
                    command.Notification = null;
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (_myConnection.State == ConnectionState.Closed)
                        _myConnection.Open();

                    using (var reader = command.ExecuteReader())
                        return reader.Cast<IDataRecord>()
                            .Select(x => new SNMPData()
                            {
                                ID = x.GetInt32(0),
                                IPAddress = x.GetString(1),
                                OID = x.GetString(2),
                                Result = x.GetString(3)
                            }).ToList();
                }
            }
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            ;
        }
    }
}