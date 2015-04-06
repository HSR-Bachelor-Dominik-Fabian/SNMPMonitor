using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SNMPMontor.DataLayer;

namespace SNMPMonitor.DataLayer
{
    public class DatabaseConnectionMonitor
    {
        private SqlConnection _myConnection;
        private DatabaseSettings _databaseSettings;
        private readonly string _connectionString;

        public DatabaseConnectionMonitor(string connectionString)
        {
            _connectionString = connectionString;
            EstablishSQLConnection();
        }

        private void EstablishSQLConnection()
        {
            _databaseSettings = new DatabaseSettings(_connectionString);
            _myConnection = new SqlConnection(_databaseSettings.ConnectionString);
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

        public void AddAgentToDatabase(AgentModel agent)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveAgentCommand = new SqlCommand("INSERT INTO Agent(AgentNr, Name, IPAddress, Port, TypeNr) VALUES(@agentNr, @name, @iPAddress, @port, @typeNr)", _myConnection);
                SqlParameter paramAgentNr = new SqlParameter("@agentNr", agent.AgentNr);
                SqlParameter paramName = new SqlParameter("@name", agent.Name);
                SqlParameter paramIPAddress = new SqlParameter("@iPAddress", agent.IPAddress);
                SqlParameter paramPort = new SqlParameter("@port", agent.Port);
                SqlParameter paramTypeNr = new SqlParameter("@typeNr", agent.TypeNr);
                saveAgentCommand.Parameters.Add(paramAgentNr);
                saveAgentCommand.Parameters.Add(paramName);
                saveAgentCommand.Parameters.Add(paramIPAddress);
                saveAgentCommand.Parameters.Add(paramPort);
                saveAgentCommand.Parameters.Add(paramTypeNr);
                saveAgentCommand.ExecuteNonQuery();

                _myConnection.Close();
            }
            catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.ToString());
            }
        }

        public void AddAgentToDatabaseForDemo(string name, string iPAddress, int port)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveAgentCommand = new SqlCommand("EXEC saveAgentForDemo @Name = @name, @IPAddress = @iPAddress, @Port = @port" ,_myConnection);
                SqlParameter paramName = new SqlParameter("@name", name);
                SqlParameter paramIP = new SqlParameter("@iPAddress", iPAddress);
                SqlParameter paramPort = new SqlParameter("@port", port);
                saveAgentCommand.Parameters.Add(paramName);
                saveAgentCommand.Parameters.Add(paramIP);
                saveAgentCommand.Parameters.Add(paramPort);
                saveAgentCommand.ExecuteNonQuery();

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