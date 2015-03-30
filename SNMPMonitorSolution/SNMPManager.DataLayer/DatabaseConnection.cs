using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SNMPManager.DataLayer;

namespace SNMPManager.DataLayer
{
    public class DatabaseConnection
    {
        private SqlConnection _myConnection;
        private readonly DatabaseSettings _databaseSettings;

        public DatabaseConnection(DatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
            EstablishSQLConnection();
        }

        private void EstablishSQLConnection()
        {
            _myConnection = new SqlConnection("User id=" + _databaseSettings.User + ";" +
                           "Password=" + _databaseSettings.Password + ";Data Source=tcp:" + _databaseSettings.Location + "," + _databaseSettings.Port + ";" +
                           "Trusted_Connection=yes;integrated security=False;" +
                           "database=" + _databaseSettings.DatabaseName + "; " +
                           "connection timeout=5");
        }

        public List<AgentModel> GetAgentsFromDatabase()
        {
            List<AgentModel> agentList = new List<AgentModel>();
            _myConnection.Open();
            SqlDataReader mMyReader = GetDataFromDatabase("Agent");
            while(mMyReader.Read())
            {
                agentList.Add(new AgentModel((int) mMyReader["AgentNr"], mMyReader["Name"].ToString(), mMyReader["IPAddress"].ToString(), (int) mMyReader["TypeNr"], (int) mMyReader["Port"]));
            }
            _myConnection.Close();
            return agentList;
        }
        
        public List<MonitoringTypeModel> GetMonitoringTypesForAgentFromDatabase(int agentNr)
        {
            List<MonitoringTypeModel> monitoringTypeList = new List<MonitoringTypeModel>();
            List<int> monitoringTypeNrForAgent = new List<int>();
            _myConnection.Open();
            SqlDataReader agentToMonitoringTypeSet = GetDataFromDatabaseWithField("AgentToMonitoringType", "AgentNr", agentNr);

            while(agentToMonitoringTypeSet.Read())
            {
                monitoringTypeNrForAgent.Add((int)agentToMonitoringTypeSet["MonitoringTypeNr"]);
            }
            _myConnection.Close();

            foreach (int monitoringTypeNr in monitoringTypeNrForAgent)
            {
                _myConnection.Open();
                SqlDataReader mMonitoringTypeSet = GetDataFromDatabaseWithField("MonitoringType", "MonitoringTypeNr", monitoringTypeNr);
                while (mMonitoringTypeSet.Read())
                {
                    monitoringTypeList.Add(new MonitoringTypeModel((int)mMonitoringTypeSet["MonitoringTypeNr"], mMonitoringTypeSet["Description"].ToString(), mMonitoringTypeSet["ObjectID"].ToString()));
                }
                _myConnection.Close();
            }
            return monitoringTypeList;
        }

        public List<TypeModel> GetTypesFromDatabase()
        {
            List<TypeModel> typeList = new List<TypeModel>();
            SqlDataReader myReader = GetDataFromDatabase("Type");
            while(myReader.Read())
            {
                typeList.Add(new TypeModel((int) myReader["TypeNr"], myReader["Name"].ToString()));
            }
            _myConnection.Close();
            return typeList;
        }

        private SqlDataReader GetDataFromDatabase(String DatabaseName)
        {
            SqlDataReader myReader = null;
            try
            {
                SqlCommand myCommand = new SqlCommand("SELECT * FROM " + DatabaseName, _myConnection);
                myReader = myCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return myReader;
        }

        private SqlDataReader GetDataFromDatabaseWithField(String DatabaseName, String FieldName, int ID)
        {
            SqlDataReader myReader = null;
            try
            {
                SqlCommand myCommand = new SqlCommand("SELECT * FROM " + DatabaseName + " WHERE " + FieldName + " = @ID", _myConnection);
                myCommand.Parameters.Add(new SqlParameter("ID", ID));
                myReader = myCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return myReader;
        }

        public void SaveMonitorDataToDatabase(int agentNr, String monitoringTypeNrString, String result)
        {
            try
            {
                _myConnection.Open();

                SqlCommand getMoniTypeNrCommand = new SqlCommand("SELECT * FROM MonitoringType WHERE ObjectID = '@monitoringTypeNrString'", _myConnection);
                SqlParameter sp = new SqlParameter("monitoringTypeNrString", monitoringTypeNrString);
                getMoniTypeNrCommand.Parameters.Add(sp);
                SqlDataReader myReader = getMoniTypeNrCommand.ExecuteReader();
                myReader.Read();
                int monitoringTypeNr = (int)myReader["MonitoringTypeNr"];
                myReader.Close();

                SqlCommand myCommand = new SqlCommand("INSERT INTO MonitorData(Result, AgentNr, MonitoringTypeNr) VALUES('" + result + "', '" + agentNr + "', '" + monitoringTypeNr + "')", _myConnection);
                myCommand.ExecuteNonQuery();
                _myConnection.Close();
            }
            catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.ToString());
            }
        }
    }
}
