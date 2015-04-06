using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SnmpSharpNet;
using SNMPManager.DataLayer;

namespace SNMPManager.BusinessLayer
{
    public class SNMPController
    {
        private readonly string _connectionString;

        public SNMPController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveSNMPDataFromAgentsToDatabase()
        {
            DatabaseConnectionManager connection = new DatabaseConnectionManager(_connectionString);
            List<AgentModel> AgentList = connection.GetAgentsFromDatabase();

            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = SnmpVersion.Ver2;
            
            foreach (AgentModel agent in AgentList)
            {
                IpAddress agentIpAddress = new IpAddress(agent.IPAddress);
                UdpTarget target = new UdpTarget((IPAddress)agentIpAddress, agent.Port, 2000, 1);

                List<MonitoringTypeModel> MonitoringTypeList = connection.GetMonitoringTypesForAgentFromDatabase(agent.AgentNr);
                
                Pdu pdu = new Pdu(PduType.Get);
                foreach (MonitoringTypeModel MonitoringType in MonitoringTypeList)
                {
                    pdu.VbList.Add(MonitoringType.ObjectID);
                }

                SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
                
                if (result != null)
                {
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        Console.WriteLine("Error in SNMP reply. Error {0} index {1}", result.Pdu.ErrorStatus, result.Pdu.ErrorIndex);
                    }
                    else
                    {
                        for (int i = 0; i < result.Pdu.VbList.Count(); i++)
                        {
                            connection.SaveMonitorDataToDatabase(agent, result.Pdu.VbList[i].Oid.ToString(), result.Pdu.VbList[i].Value.ToString());
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No response recieved from SNMP Agent");
                }
                
                target.Close();
                Console.ReadLine();
            }
        }
    }
}