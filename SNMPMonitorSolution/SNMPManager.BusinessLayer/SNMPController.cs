using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SnmpSharpNet;
using SNMPManager.DataLayer;
using System.Diagnostics;

namespace SNMPManager.BusinessLayer
{
    public class SNMPController
    {
        private readonly string _connectionString;
        private DatabaseConnectionManager connection;

        public SNMPController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GetSNMPDataFromAgents(Boolean isLongTimeCheck)
        {
            connection = new DatabaseConnectionManager(_connectionString);
            List<AgentDataModel> AgentList = connection.GetAgentsFromDatabase();

            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = SnmpVersion.Ver2;
            
            foreach (AgentDataModel agent in AgentList)
            {
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
        }

        private Pdu GetPduForAgent(Boolean isLongTimeCheck, AgentDataModel agent)
        {
            List<MonitoringTypeDataModel> MonitoringTypeList = connection.GetMonitoringTypesForAgentFromDatabase(agent.AgentNr);

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
            if (isLongTimeCheck)
            {
                connection.SaveLongTimeMonitorDataToDatabase(agent, resultList);
            }
            else
            {
                connection.SaveMonitorDataToDatabase(agent, resultList);
            }
        }
    }
}