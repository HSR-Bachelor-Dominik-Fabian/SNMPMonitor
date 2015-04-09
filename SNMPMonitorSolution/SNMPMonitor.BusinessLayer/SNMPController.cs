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

        public List<MonitorData> GetHistoryOfOIDForAgent(int agentNr, string ObjectID)
        {
            List<MonitorData> monitoringDataList = new List<MonitorData>();
            List<MonitorDataDataModel> resultList = _databaseConnection.GetHistoryOfOIDForAgent(agentNr, ObjectID);
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
            foreach (AgentDataModel agentData in resultList) 
            {
                agentList.Add(new Agent(agentData.AgentNr, agentData.Name, agentData.IPAddress, agentData.TypeNr, agentData.Port, agentData.Status));
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
            AgentDataModel agentData = new AgentDataModel(agent.AgentNr, agent.Name, agent.IPAddress, agent.TypeNr, agent.Port, agent.Status);
            _databaseConnection.AddAgentToDatabase(agentData);
        }
    }
}