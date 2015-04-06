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
            try
            {
                _myConnection.Open();

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("EXEC getMonitoringTypesForAgent @AgentNr = @agentNr", _myConnection);
                getMonitoringTypesForAgent.Parameters.Add(new SqlParameter("@agentNr", agentNr));

                SqlDataReader myMonitoringTypeSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myMonitoringTypeSet.Read())
                {
                    monitoringTypeList.Add(new MonitoringTypeModel((int)myMonitoringTypeSet["MonitoringTypeNr"], myMonitoringTypeSet["Description"].ToString(), myMonitoringTypeSet["ObjectID"].ToString()));
                }
                _myConnection.Close();
            } catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.StackTrace.ToString());
            }
            return monitoringTypeList;
        }

        public List<TypeModel> GetTypesFromDatabase()
        {
            _myConnection.Open();

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
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM " + DatabaseName, _myConnection);
                //selectCommand.Parameters.Add(new SqlParameter("@databaseName", DatabaseName));
                myReader = selectCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace.ToString());
            }
            return myReader;
        }

        public void AddAgentToDatabase(AgentModel agent)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveAgentCommand = new SqlCommand("INSERT INTO Agent(AgentNr, Name, IPAddress, Port, TypeNr) VALUES(@agentNr, @name, @iPAddress, @port, @typeNr)", _myConnection);
                saveAgentCommand.Parameters.Add(new SqlParameter("@agentNr", agent.AgentNr));
                saveAgentCommand.Parameters.Add(new SqlParameter("@name", agent.Name));
                saveAgentCommand.Parameters.Add(new SqlParameter("@iPAddress", agent.IPAddress));
                saveAgentCommand.Parameters.Add(new SqlParameter("@port", agent.Port));
                saveAgentCommand.Parameters.Add(new SqlParameter("@typeNr", agent.TypeNr));
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