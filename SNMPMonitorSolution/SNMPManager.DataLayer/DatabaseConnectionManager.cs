using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SNMPManager.DataLayer;
using System.Diagnostics;

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
            try
            {
                _myConnection.Open();

                SqlCommand getAgents = new SqlCommand("getAgents", _myConnection);
                getAgents.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader myAgentsSet = getAgents.ExecuteReader();

                while (myAgentsSet.Read())
                {
                    agentList.Add(new AgentDataModel((int)myAgentsSet["AgentNr"], myAgentsSet["Name"].ToString(), myAgentsSet["IPAddress"].ToString(), (int)myAgentsSet["TypeNr"], (int)myAgentsSet["Port"], (int)myAgentsSet["Status"], myAgentsSet["sysDesc"].ToString(), myAgentsSet["sysName"].ToString(), myAgentsSet["sysUptime"].ToString()));
                }
            }
            finally
            {
                _myConnection.Close();
            }
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
            }
            finally
            {
                _myConnection.Close();
            }
            return monitoringTypeList;
        }
        
        public List<MonitoringTypeDataModel> GetMonitoringTypesForAgentForCheckFromDatabase(int agentNr)
        {
            List<MonitoringTypeDataModel> monitoringTypeList = new List<MonitoringTypeDataModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getMonitoringTypesForAgentForCheck", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;
                getMonitoringTypesForAgent.Parameters.Add(new SqlParameter("@AgentNr", agentNr));

                SqlDataReader myMonitoringTypeSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myMonitoringTypeSet.Read())
                {
                    monitoringTypeList.Add(new MonitoringTypeDataModel((int)myMonitoringTypeSet["MonitoringTypeNr"], myMonitoringTypeSet["Description"].ToString(), myMonitoringTypeSet["ObjectID"].ToString()));
                }
            }
            finally
            {
                _myConnection.Close();
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
            }
            finally
            {
                _myConnection.Close();
            }
            return typeList;
        }

        public void AddMonitorDataToDatabase(AgentDataModel agent, List<KeyValuePair<string, string>> resultSet)
        {
            try
            {
                _myConnection.Open();
                foreach (KeyValuePair<string, string> result in resultSet)
                {
                    SqlCommand saveMonitorDataCommand = new SqlCommand("addMonitorData", _myConnection);
                    saveMonitorDataCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    saveMonitorDataCommand.Parameters.Add(new SqlParameter("@MonitoringTypeNr", result.Key));
                    saveMonitorDataCommand.Parameters.Add(new SqlParameter("@Result", result.Value));
                    saveMonitorDataCommand.Parameters.Add(new SqlParameter("@AgentNr", agent.AgentNr));

                    saveMonitorDataCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void AddAgentToDatabase(AgentDataModel agent)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveAgentCommand = new SqlCommand("addAgent", _myConnection);
                saveAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                saveAgentCommand.Parameters.Add(new SqlParameter("@Name", agent.Name));
                saveAgentCommand.Parameters.Add(new SqlParameter("@IPAddress", agent.IPAddress));
                saveAgentCommand.Parameters.Add(new SqlParameter("@Port", agent.Port));
                saveAgentCommand.Parameters.Add(new SqlParameter("@TypeNr", agent.TypeNr));
                saveAgentCommand.Parameters.Add(new SqlParameter("@StatusNr", agent.Status));
                saveAgentCommand.ExecuteNonQuery();

            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void AddEventToDatabase(string exceptionType, string category, DateTime timestamp, int hResult, string message, string stackTrace)
        {
            try
            {
                _myConnection.Open();
                
                SqlCommand addEventCommand = new SqlCommand("addEvent", _myConnection);
                addEventCommand.CommandType = System.Data.CommandType.StoredProcedure;
                addEventCommand.Parameters.Add(new SqlParameter("@ExceptionType", exceptionType));
                addEventCommand.Parameters.Add(new SqlParameter("@Category", category));
                addEventCommand.Parameters.Add(new SqlParameter("@EventTimestamp", timestamp));
                addEventCommand.Parameters.Add(new SqlParameter("@HResult", hResult));
                addEventCommand.Parameters.Add(new SqlParameter("@Message", message));
                addEventCommand.Parameters.Add(new SqlParameter("@Stacktrace", stackTrace));
                addEventCommand.ExecuteNonQuery();
                
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void UpdateStatusOfAgent(int agentNr, int status)
        {
            try
            {
                _myConnection.Open();

                SqlCommand updateStatusOfAgentCommand = new SqlCommand("updateStatusOfAgent", _myConnection);
                updateStatusOfAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                updateStatusOfAgentCommand.Parameters.Add(new SqlParameter("@AgentNr", agentNr));
                updateStatusOfAgentCommand.Parameters.Add(new SqlParameter("@Status", status));
                updateStatusOfAgentCommand.ExecuteNonQuery();

            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void DeleteAgentInDatabase(int agentNr)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveAgentCommand = new SqlCommand("deleteAgent", _myConnection);
                saveAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                saveAgentCommand.Parameters.Add(new SqlParameter("@AgentNr", agentNr));
                saveAgentCommand.ExecuteNonQuery();

            }
            finally
            {
                _myConnection.Close();
            }
        }
    }
}
