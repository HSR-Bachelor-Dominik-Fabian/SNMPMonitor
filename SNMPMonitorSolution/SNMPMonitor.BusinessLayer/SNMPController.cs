using SNMPMonitor.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    public class SNMPController
    {
        private readonly DatabaseConnectionMonitor _databaseConnection;

        public SNMPController(string connectionString) {
            _databaseConnection = new DatabaseConnectionMonitor(connectionString);
        }

        public List<MonitoringType> GetMonitoringTypesForAgent(int agentNr)
        {
            List<MonitoringType> monitoringTypeList = new List<MonitoringType>();
            List<MonitoringTypeDataModel> resultList = _databaseConnection.GetMonitoringTypesForAgentFromDatabase(agentNr);
            foreach (MonitoringTypeDataModel monitoringType in resultList)
            {
                monitoringTypeList.Add(new MonitoringType(monitoringType.MonitoringTypeNr, monitoringType.Description, monitoringType.ObjectID));
            }
            return monitoringTypeList;
        }

        public List<MonitorData> GetHistoryOfOIDForAgent(int agentNr, string ObjectID, int count)
        {
            List<MonitorData> monitoringDataList = new List<MonitorData>();
            List<MonitorDataDataModel> resultList = _databaseConnection.GetHistoryOfOIDForAgent(agentNr, ObjectID, count);
            foreach (MonitorDataDataModel monitoringData in resultList)
            {
                monitoringDataList.Add(new MonitorData(monitoringData.Timestamp, monitoringData.Result, monitoringData.AgentNr, monitoringData.ObjectID));
            }
            return monitoringDataList;
        }

        public void AddAgentToDatabaseForDemo(string name, string iPAddress, int port)
        {
            _databaseConnection.AddAgentToDatabaseForDemo(name, iPAddress, port);
        }

        public List<Agent> GetAgents()
        {
            List<Agent> agentList = new List<Agent>();
            List<AgentDataModel> resultList = _databaseConnection.GetAgentsFromDatabase();

            List<Type> typeList = GetTypes();

            foreach (AgentDataModel agentData in resultList) 
            {
                Type type = null;
                foreach (Type temp in typeList)
                {
                    if (temp.TypeNr == agentData.Type.TypeNr)
                    {
                        type = temp;
                    }
                }
                agentList.Add(new Agent(agentData.AgentNr, agentData.Name, agentData.IPAddress, type, agentData.Port, agentData.Status, agentData.SysDescription, agentData.SysName, agentData.SysUptime));
            }
            return agentList;
        }

        public List<Type> GetTypes()
        {
            List<Type> typeList = new List<Type>();
            List<TypeDataModel> resultList = _databaseConnection.GetTypesFromDatabase();
            foreach (TypeDataModel typeData in resultList)
            {
                typeList.Add(new Type(typeData.TypeNr, typeData.Name));
            }
            return typeList;
        }

        public void AddAgentToDatabase(Agent agent)
        {
            AgentDataModel agentData = new AgentDataModel(agent.AgentNr, agent.Name, agent.IPAddress, new TypeDataModel(agent.Type.TypeNr, agent.Type.Name), agent.Port, agent.Status, "undefined", "undefined", "undefined");
            _databaseConnection.AddAgentToDatabase(agentData);
        }

        public List<KeyValuePair<Agent, List<MonitoringType>>> GetMonitoringSummary()
        {
            List<KeyValuePair<Agent, List<MonitoringType>>> returnList = new List<KeyValuePair<Agent, List<MonitoringType>>>();
            List<Agent> agentList = GetAgents();
            foreach (Agent agent in agentList)
            {
                List<MonitoringType> monitoringTypes = GetMonitoringTypesForAgent(agent.AgentNr);
                returnList.Add(new KeyValuePair<Agent, List<MonitoringType>>(agent, monitoringTypes));
            }
            return returnList;
        }
    }
}