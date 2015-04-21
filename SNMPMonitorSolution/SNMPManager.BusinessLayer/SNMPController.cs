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

        public SNMPController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GetSNMPDataFromAgents()
        {
            DatabaseConnectionManager connection = new DatabaseConnectionManager(_connectionString);
            List<AgentDataModel> AgentList = connection.GetAgentsFromDatabase();

            Parallel.ForEach(AgentList, agent => GetSNMPDataFromSingleAgent(agent));
        }

        private void GetSNMPDataFromSingleAgent(AgentDataModel agent)
        {
            DatabaseConnectionManager connection = new DatabaseConnectionManager(_connectionString);

            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = SnmpVersion.Ver2;

            IpAddress agentIpAddress = new IpAddress(agent.IPAddress);
            UdpTarget target = new UdpTarget((IPAddress)agentIpAddress, agent.Port, 2000, 1);

            Pdu pdu = GetPduForAgent(agent, connection);

            SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);

            if (result != null)
            {
                if (result.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Error in SNMP reply. Error {0} index {1}", result.Pdu.ErrorStatus, result.Pdu.ErrorIndex);
                }
                else
                {
                    SendDataToDataLayer(agent, result, connection);
                }
            }
            else
            {
                Console.WriteLine("No response recieved from SNMP Agent");
            }

            target.Close();
        }

        private Pdu GetPduForAgent(AgentDataModel agent, DatabaseConnectionManager connection)
        {
            List<MonitoringTypeDataModel> MonitoringTypeList = connection.GetMonitoringTypesForAgentForCheckFromDatabase(agent.AgentNr);

            Pdu pdu = new Pdu(PduType.Get);
            foreach (MonitoringTypeDataModel MonitoringType in MonitoringTypeList)
            {
                pdu.VbList.Add(MonitoringType.ObjectID);
            }
            return pdu;
        }

        private void SendDataToDataLayer(AgentDataModel agent, SnmpV2Packet result, DatabaseConnectionManager connection)
        {
            List<KeyValuePair<string, string>> resultList = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < result.Pdu.VbList.Count(); i++)
            {
                resultList.Add(new KeyValuePair<string, string>(result.Pdu.VbList[i].Oid.ToString(), result.Pdu.VbList[i].Value.ToString()));
            }
            connection.AddMonitorDataToDatabase(agent, resultList);
        }
    }
}