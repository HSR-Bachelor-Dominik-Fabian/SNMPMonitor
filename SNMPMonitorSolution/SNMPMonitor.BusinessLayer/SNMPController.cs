using SNMPMonitor.BusinessLayer.ExceptionHandling;
using SNMPMonitor.DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            try
            {
                List<MonitoringTypeDataModel> resultList = _databaseConnection.GetMonitoringTypesForAgentFromDatabase(agentNr);
                foreach (MonitoringTypeDataModel monitoringType in resultList)
                {
                    monitoringTypeList.Add(new MonitoringType(monitoringType.MonitoringTypeNr, monitoringType.Description, monitoringType.ObjectID));
                }
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (InvalidCastException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
            return monitoringTypeList;
        }

        public List<MonitorData> GetHistoryOfOIDForAgent(int agentNr, string ObjectID, int count)
        {
            List<MonitorData> monitoringDataList = new List<MonitorData>();
            try
            {
                List<MonitorDataDataModel> resultList = _databaseConnection.GetHistoryOfOIDForAgent(agentNr, ObjectID, count);
                foreach (MonitorDataDataModel monitoringData in resultList)
                {
                    monitoringDataList.Add(new MonitorData(monitoringData.Timestamp, monitoringData.Result, monitoringData.AgentNr, monitoringData.ObjectID));
                }
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (InvalidCastException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
            return monitoringDataList;
        }

        public void AddAgentToDatabaseForDemo(string name, string iPAddress, int port)
        {
            try
            {
                _databaseConnection.AddAgentToDatabaseForDemo(name, iPAddress, port);
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
        }

        public List<Agent> GetAgents()
        {
            List<Agent> agentList = new List<Agent>();
            try
            {
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
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (InvalidCastException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
            return agentList;
        }

        public List<Type> GetTypes()
        {
            List<Type> typeList = new List<Type>();
            try
            {
                List<TypeDataModel> resultList = _databaseConnection.GetTypesFromDatabase();
                foreach (TypeDataModel typeData in resultList)
                {
                    typeList.Add(new Type(typeData.TypeNr, typeData.Name));
                }
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (InvalidCastException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
            return typeList;
        }

        public void AddAgentToDatabase(Agent agent)
        {
            AgentDataModel agentData = new AgentDataModel(agent.AgentNr, agent.Name, agent.IPAddress, new TypeDataModel(agent.Type.TypeNr, agent.Type.Name), agent.Port, agent.Status, "undefined", "undefined", "undefined");
            try
            {
                _databaseConnection.AddAgentToDatabase(agentData);
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }

        }

        public List<KeyValuePair<Agent, List<MonitoringType>>> GetMonitoringSummary()
        {
            List<KeyValuePair<Agent, List<MonitoringType>>> returnList = new List<KeyValuePair<Agent, List<MonitoringType>>>();
            try
            {
                List<Agent> agentList = GetAgents();
                foreach (Agent agent in agentList)
                {
                    List<MonitoringType> monitoringTypes = GetMonitoringTypesForAgent(agent.AgentNr);
                    returnList.Add(new KeyValuePair<Agent, List<MonitoringType>>(agent, monitoringTypes));
                }
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (InvalidCastException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
            return returnList;
        }

        public List<Event> GetAllEvents()
        {
            List<Event> eventList = new List<Event>();
            try
            {
                List<EventDataModel> resultList = _databaseConnection.GetAllEventsFromDatabase();
                foreach (EventDataModel eventData in resultList)
                {
                    eventList.Add(new Event(eventData.EventNr, eventData.ExceptionType, eventData.Category, eventData.EventTimestamp, eventData.HResult, eventData.Message, eventData.Stacktrace));
                }
            }
            catch (SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (InvalidCastException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
            return eventList;
        }
    }
}