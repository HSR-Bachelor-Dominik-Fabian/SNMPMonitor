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
    }
}
