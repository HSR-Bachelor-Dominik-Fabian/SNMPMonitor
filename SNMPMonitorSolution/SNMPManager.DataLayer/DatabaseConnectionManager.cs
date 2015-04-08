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

        public List<AgentDataModel> GetAgentsFromDatabase()
        {
            List<AgentDataModel> agentList = new List<AgentDataModel>();
            _myConnection.Open();

            SqlCommand getMonitoringTypesForAgent = new SqlCommand("getAgents", _myConnection);
            getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader myAgentsSet = getMonitoringTypesForAgent.ExecuteReader();

            while (myAgentsSet.Read())
            {
                agentList.Add(new AgentDataModel((int)myAgentsSet["AgentNr"], myAgentsSet["Name"].ToString(), myAgentsSet["IPAddress"].ToString(), (int)myAgentsSet["TypeNr"], (int)myAgentsSet["Port"], (int)myAgentsSet["Status"]));
            }
            _myConnection.Close();
            return agentList;
        }
        
        public List<MonitoringTypeDataModel> GetMonitoringTypesForAgentFromDatabase(int agentNr)
        {
            List<MonitoringTypeDataModel> monitoringTypeList = new List<MonitoringTypeDataModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getMonitoringTypesForAgent", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;
                getMonitoringTypesForAgent.Parameters.Add(new SqlParameter("@AgentNr", agentNr));

                SqlDataReader myMonitoringTypeSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myMonitoringTypeSet.Read())
                {
                    monitoringTypeList.Add(new MonitoringTypeDataModel((int)myMonitoringTypeSet["MonitoringTypeNr"], myMonitoringTypeSet["Description"].ToString(), myMonitoringTypeSet["ObjectID"].ToString()));
                }
                _myConnection.Close();
            } catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.StackTrace.ToString());
            }
            return monitoringTypeList;
        }

        public List<TypeDataModel> GetTypesFromDatabase()
        {
            List<TypeDataModel> typeList = new List<TypeDataModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getTypes", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader myTypesSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myTypesSet.Read())
                {
                    typeList.Add(new TypeDataModel((int)myTypesSet["TypeNr"], myTypesSet["Name"].ToString()));
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

        public void SaveMonitorDataToDatabase(AgentDataModel agent, String monitoringTypeNr, String result)
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


        public void AddAgentToDatabase(AgentDataModel agent)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveAgentCommand = new SqlCommand("addAgent", _myConnection);
                saveAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                saveAgentCommand.Parameters.Add(new SqlParameter("@AgentNr", agent.AgentNr));
                saveAgentCommand.Parameters.Add(new SqlParameter("@Name", agent.Name));
                saveAgentCommand.Parameters.Add(new SqlParameter("@IPAddress", agent.IPAddress));
                saveAgentCommand.Parameters.Add(new SqlParameter("@Port", agent.Port));
                saveAgentCommand.Parameters.Add(new SqlParameter("@TypeNr", agent.TypeNr));
                saveAgentCommand.Parameters.Add(new SqlParameter("@StatusNr", agent.Status));
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
