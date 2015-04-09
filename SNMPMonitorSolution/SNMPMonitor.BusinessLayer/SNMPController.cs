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
    }
}
