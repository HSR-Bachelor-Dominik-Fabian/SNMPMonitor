using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SNMPManager.DataLayer;

namespace SNMPManager.DataLayer
{
    public class DatabaseConnectionManager
    {
        private SqlConnection _myConnection;
        private DatabaseSettings _databaseSettings;
        private readonly string _connectionString;

        public DatabaseConnectionManager(string connectionString)
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

            SqlCommand getMonitoringTypesForAgent = new SqlCommand("getAgents", _myConnection);
            getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader myAgentsSet = getMonitoringTypesForAgent.ExecuteReader();

            while (myAgentsSet.Read())
            {
                agentList.Add(new AgentModel((int)myAgentsSet["AgentNr"], myAgentsSet["Name"].ToString(), myAgentsSet["IPAddress"].ToString(), (int)myAgentsSet["TypeNr"], (int)myAgentsSet["Port"]));
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

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getMonitoringTypesForAgent", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;
                getMonitoringTypesForAgent.Parameters.Add(new SqlParameter("@AgentNr", agentNr));

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

        public void SaveMonitorDataToDatabase(AgentModel agent, String monitoringTypeNr, String result)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveMonitorDataCommand = new SqlCommand("saveMonitorData", _myConnection);
                saveMonitorDataCommand.CommandType = System.Data.CommandType.StoredProcedure;
                saveMonitorDataCommand.Parameters.Add(new SqlParameter("@MonitoringTypeNr", monitoringTypeNr));
                saveMonitorDataCommand.Parameters.Add(new SqlParameter("@Result", result));
                saveMonitorDataCommand.Parameters.Add(new SqlParameter("@AgentNr", agent.AgentNr));

                saveMonitorDataCommand.ExecuteNonQuery();
                _myConnection.Close();
            }
            catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.StackTrace.ToString());
            }
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
    }
}
