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
            try
            {
                _myConnection.Open();

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getAgents", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader myAgentsSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myAgentsSet.Read())
                {
                    agentList.Add(new AgentModel((int)myAgentsSet["AgentNr"], myAgentsSet["Name"].ToString(), myAgentsSet["IPAddress"].ToString(), (int)myAgentsSet["TypeNr"], (int)myAgentsSet["Port"]));
                }
                _myConnection.Close();
            }
            catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.StackTrace.ToString());
            }
            return agentList;
        }
        
        public List<MonitoringTypeModel> GetMonitoringTypesForAgentFromDatabase(int agentNr)
        {
            List<MonitoringTypeModel> monitoringTypeList = new List<MonitoringTypeModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getMonitoringTypesForAgent", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;
                getMonitoringTypesForAgent.Parameters.Add(new SqlParameter("@AgentNr", agentNr));

                SqlDataReader myMonitoringTypeSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myMonitoringTypeSet.Read())
                {
                    monitoringTypeList.Add(new MonitoringTypeModel((int)myMonitoringTypeSet["MonitoringTypeNr"], myMonitoringTypeSet["Description"].ToString(), myMonitoringTypeSet["ObjectID"].ToString()));
                }
                _myConnection.Close();
            }
            catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.StackTrace.ToString());
            }
            return monitoringTypeList;
        }

        public List<TypeModel> GetTypesFromDatabase()
        {
            List<TypeModel> typeList = new List<TypeModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getTypes", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader myTypesSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myTypesSet.Read())
                {
                    typeList.Add(new TypeModel((int)myTypesSet["TypeNr"], myTypesSet["Name"].ToString()));
                }
                _myConnection.Close();
            }
            catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.StackTrace.ToString());
            }
            return typeList;
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

                SqlCommand saveAgentCommand = new SqlCommand("saveAgentForDemo" ,_myConnection);
                saveAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter paramName = new SqlParameter("@Name", name);
                SqlParameter paramIP = new SqlParameter("@IPAddress", iPAddress);
                SqlParameter paramPort = new SqlParameter("@Port", port);
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
        /*
        public void GetSummaryOfAgent(int agentNr)
        {
            try
            {
                _myConnection.Open();
                SqlCommand getSummaryOfAgentCommand = new SqlCommand("getSummaryForAgent", _myConnection);
                getSummaryOfAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                getSummaryOfAgentCommand.Parameters.Add(new SqlParameter("@AgentNr", agentNr));
                SqlDataReader summarySetOfAgent = getSummaryOfAgentCommand.ExecuteReader();
                _myConnection.Close();

            } catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.ToString());
            }
        }
        */
    }
}