using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SnmpSharpNet;
using SNMPManager.DataLayer;
using System.Diagnostics;
using System.Threading;

namespace SNMPManager.BusinessLayer
{
    public class SNMPController
    {
        private readonly string _connectionString;
        private DatabaseConnectionManager connection;
        private Object _sync = new Object();

        public SNMPController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GetSNMPDataFromAgents(bool isLongTimeCheck)
        {
            connection = new DatabaseConnectionManager(_connectionString);
            Monitor.Enter(_sync);
            List<AgentDataModel> AgentList = connection.GetAgentsFromDatabase();
            Monitor.Exit(_sync);

            Parallel.ForEach(AgentList, agent => GetSNMPDataFromSingleAgent(agent, isLongTimeCheck));
        }

        private void GetSNMPDataFromSingleAgent(AgentDataModel agent, bool isLongTimeCheck)
        {
            //Trace.WriteLine("Task läuft: " + agent.AgentNr);

            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = SnmpVersion.Ver2;

            IpAddress agentIpAddress = new IpAddress(agent.IPAddress);
            UdpTarget target = new UdpTarget((IPAddress)agentIpAddress, agent.Port, 2000, 1);

            Pdu pdu = GetPduForAgent(isLongTimeCheck, agent);

            SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);

            if (result != null)
            {
                if (result.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Error in SNMP reply. Error {0} index {1}", result.Pdu.ErrorStatus, result.Pdu.ErrorIndex);
                }
                else
                {
                    SendDataToDataLayer(isLongTimeCheck, agent, result);
                }
            }
            else
            {
                Console.WriteLine("No response recieved from SNMP Agent");
            }

            target.Close();
        }

        private Pdu GetPduForAgent(Boolean isLongTimeCheck, AgentDataModel agent)
        {
            Monitor.Enter(_sync);
            List<MonitoringTypeDataModel> MonitoringTypeList = connection.GetMonitoringTypesForAgentFromDatabase(agent.AgentNr);
            Monitor.Exit(_sync);

            Pdu pdu = new Pdu(PduType.Get);
            foreach (MonitoringTypeDataModel MonitoringType in MonitoringTypeList)
            {
                if (isLongTimeCheck && MonitoringType.IsLongTimeCheck)
                {
                    pdu.VbList.Add(MonitoringType.ObjectID);
                }
                else if (!isLongTimeCheck && !MonitoringType.IsLongTimeCheck)
                {
                    pdu.VbList.Add(MonitoringType.ObjectID);
                }
            }
            return pdu;
        }

        private void SendDataToDataLayer(Boolean isLongTimeCheck, AgentDataModel agent, SnmpV2Packet result)
        {
            List<KeyValuePair<string, string>> resultList = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < result.Pdu.VbList.Count(); i++)
            {
                resultList.Add(new KeyValuePair<string, string>(result.Pdu.VbList[i].Oid.ToString(), result.Pdu.VbList[i].Value.ToString()));
            }
            Monitor.Enter(_sync);
            if (isLongTimeCheck)
            {
                connection.SaveLongTimeMonitorDataToDatabase(agent, resultList);
            }
            else
            {
                connection.SaveMonitorDataToDatabase(agent, resultList);
            }
            Monitor.Exit(_sync);
        }
    }
}