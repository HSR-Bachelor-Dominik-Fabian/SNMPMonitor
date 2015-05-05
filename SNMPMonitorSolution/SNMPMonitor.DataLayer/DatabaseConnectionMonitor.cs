using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SNMPMonitor.DataLayer;

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

        public List<AgentDataModel> GetAgentsFromDatabase()
        {
            List<AgentDataModel> agentList = new List<AgentDataModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getAgents = new SqlCommand("getAgents", _myConnection);
                getAgents.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader myAgentsSet = getAgents.ExecuteReader();

                List<TypeDataModel> typeSet = new DatabaseConnectionMonitor(Properties.Settings.Default.ProdDatabase).GetTypesFromDatabase();

                while (myAgentsSet.Read())
                {
                    TypeDataModel type = null;
                    int tempTypeNr = (int)myAgentsSet["TypeNr"];
                    foreach (TypeDataModel temp in typeSet)
                    {
                        if (temp.TypeNr == tempTypeNr)
                        {
                            type = temp;
                        }
                    }
                    agentList.Add(new AgentDataModel((int)myAgentsSet["AgentNr"], myAgentsSet["Name"].ToString(), myAgentsSet["IPAddress"].ToString(), type, (int)myAgentsSet["Port"], (int)myAgentsSet["Status"], myAgentsSet["sysDesc"].ToString(), myAgentsSet["sysName"].ToString(), myAgentsSet["sysUptime"].ToString()));
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
                saveAgentCommand.Parameters.Add(new SqlParameter("@TypeNr", agent.Type.TypeNr));
                saveAgentCommand.Parameters.Add(new SqlParameter("@StatusNr", agent.Status));
                saveAgentCommand.ExecuteNonQuery();
            }
            finally
            {
                _myConnection.Close();
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
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<MonitorDataDataModel> GetHistoryOfOIDForAgent(int agentNr, string ObjectID, int count)
        {
            List<MonitorDataDataModel> monitorDataList = new List<MonitorDataDataModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getHistoryOfOIDForAgentCommand = new SqlCommand("getHistoryOfOIDForAgent", _myConnection);
                getHistoryOfOIDForAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                getHistoryOfOIDForAgentCommand.Parameters.Add(new SqlParameter("@AgentNr", agentNr));
                getHistoryOfOIDForAgentCommand.Parameters.Add(new SqlParameter("@ObjectID", ObjectID));
                getHistoryOfOIDForAgentCommand.Parameters.Add(new SqlParameter("@Count", count));

                SqlDataReader myHistoryOfOidForAgentSet = getHistoryOfOIDForAgentCommand.ExecuteReader();

                while (myHistoryOfOidForAgentSet.Read())
                {
                    monitorDataList.Add(new MonitorDataDataModel((DateTime)myHistoryOfOidForAgentSet["MonitorTimestamp"], myHistoryOfOidForAgentSet["Result"].ToString(), (int)myHistoryOfOidForAgentSet["AgentNR"], myHistoryOfOidForAgentSet["ObjectID"].ToString()));
                }
            }
            finally
            {
                _myConnection.Close();
            }
            return monitorDataList;
        }

        public void AddEventToDatabase(string exceptionType, string category, string timestamp, string hResult, string message, string stackTrace)
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
    }
}