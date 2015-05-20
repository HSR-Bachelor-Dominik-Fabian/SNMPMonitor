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

        public AgentDataModel GetAgentFromDatabase(int agentNr)
        {
            AgentDataModel agentList = null;
            try
            {
                _myConnection.Open();

                SqlCommand getAgent = new SqlCommand("getAgent", _myConnection);
                getAgent.CommandType = System.Data.CommandType.StoredProcedure;
                getAgent.Parameters.Add(new SqlParameter("@AgentNr", agentNr));

                SqlDataReader myAgentsSet = getAgent.ExecuteReader();

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
                    agentList = new AgentDataModel((int)myAgentsSet["AgentNr"], myAgentsSet["Name"].ToString(), myAgentsSet["IPAddress"].ToString(), type, (int)myAgentsSet["Port"], (int)myAgentsSet["Status"], myAgentsSet["sysDesc"].ToString(), myAgentsSet["sysName"].ToString(), myAgentsSet["sysUptime"].ToString());
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

        public void AddAgentToDatabase(AgentDataModel agent, bool cpuCheck, bool discCheck)
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
                saveAgentCommand.Parameters.Add(new SqlParameter("@CPUCheck", cpuCheck));
                saveAgentCommand.Parameters.Add(new SqlParameter("@DiscCheck", discCheck));
                saveAgentCommand.ExecuteNonQuery();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void UpdateAgentInDatabase(AgentDataModel agent, bool cpuCheck, bool discCheck)
        {
            try
            {
                _myConnection.Open();

                SqlCommand saveAgentCommand = new SqlCommand("updateAgent", _myConnection);
                saveAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                saveAgentCommand.Parameters.Add(new SqlParameter("@AgentNr", agent.AgentNr));
                saveAgentCommand.Parameters.Add(new SqlParameter("@Name", agent.Name));
                saveAgentCommand.Parameters.Add(new SqlParameter("@IPAddress", agent.IPAddress));
                saveAgentCommand.Parameters.Add(new SqlParameter("@Port", agent.Port));
                saveAgentCommand.Parameters.Add(new SqlParameter("@TypeNr", agent.Type.TypeNr));
                saveAgentCommand.Parameters.Add(new SqlParameter("@CPUCheck", cpuCheck));
                saveAgentCommand.Parameters.Add(new SqlParameter("@DiscCheck", discCheck));
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

        public void AddEventToDatabase(string exceptionType, string category, DateTime timestamp, string hResult, string message, string stackTrace)
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

        public List<EventDataModel> GetAllEventsFromDatabase()
        {
            List<EventDataModel> eventList= new List<EventDataModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getEvents = new SqlCommand("getAllEvents", _myConnection);
                getEvents.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader myEventsSet = getEvents.ExecuteReader();

                while (myEventsSet.Read())
                {
                    eventList.Add(new EventDataModel((int)myEventsSet["EventNr"], myEventsSet["ExceptionType"].ToString(), myEventsSet["Category"].ToString(), (DateTime)myEventsSet["EventTimestamp"], (int)myEventsSet["HResult"], myEventsSet["Message"].ToString(), myEventsSet["Stacktrace"].ToString()));
                }
            }
            finally
            {
                _myConnection.Close();
            }
            return eventList;
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