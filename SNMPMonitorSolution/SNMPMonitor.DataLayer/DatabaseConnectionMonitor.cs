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

                SqlCommand getMonitoringTypesForAgent = new SqlCommand("getAgents", _myConnection);
                getMonitoringTypesForAgent.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader myAgentsSet = getMonitoringTypesForAgent.ExecuteReader();

                while (myAgentsSet.Read())
                {
                    agentList.Add(new AgentDataModel((int)myAgentsSet["AgentNr"], myAgentsSet["Name"].ToString(), myAgentsSet["IPAddress"].ToString(), (int)myAgentsSet["TypeNr"], (int)myAgentsSet["Port"], (int)myAgentsSet["Status"]));
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
                    monitoringTypeList.Add(new MonitoringTypeDataModel((int)myMonitoringTypeSet["MonitoringTypeNr"], myMonitoringTypeSet["Description"].ToString(), myMonitoringTypeSet["ObjectID"].ToString(), (Boolean) myMonitoringTypeSet["IsLongTimeCheck"]));
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

        public List<MonitorDataDataModel> GetHistoryOfOIDForAgent(int agentNr, string ObjectID)
        {
            List<MonitorDataDataModel> monitorDataList = new List<MonitorDataDataModel>();
            try
            {
                _myConnection.Open();

                SqlCommand getHistoryOfOIDForAgentCommand = new SqlCommand("getHistoryOfOIDForAgent", _myConnection);
                getHistoryOfOIDForAgentCommand.CommandType = System.Data.CommandType.StoredProcedure;
                getHistoryOfOIDForAgentCommand.Parameters.Add(new SqlParameter("@AgentNr", agentNr));
                getHistoryOfOIDForAgentCommand.Parameters.Add(new SqlParameter("@ObjectID", ObjectID));

                SqlDataReader myHistoryOfOidForAgentSet = getHistoryOfOIDForAgentCommand.ExecuteReader();

                while (myHistoryOfOidForAgentSet.Read())
                {
                    monitorDataList.Add(new MonitorDataDataModel((DateTime)myHistoryOfOidForAgentSet["MonitorTimestamp"], myHistoryOfOidForAgentSet["Result"].ToString(), (int)myHistoryOfOidForAgentSet["AgentNR"], myHistoryOfOidForAgentSet["ObjectID"].ToString()));
                }

                _myConnection.Close();
            }
            catch (Exception e)
            {
                _myConnection.Close();
                Console.WriteLine(e.StackTrace.ToString());
            }
            return monitorDataList;
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