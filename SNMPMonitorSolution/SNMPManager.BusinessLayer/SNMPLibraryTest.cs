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
    class SNMPLibraryTest
    {
        static void Main(string[] args)
        {
            GetSNMPDataFromAgents();
        }

        public static void GetSNMPDataFromAgents()
        {
            DatabaseSettings databaseSettings = new DatabaseSettings("152.96.56.75", 40003, "Manager", "HSR-00228866", "SNMPMonitor");
            DatabaseConnection connection = new DatabaseConnection(databaseSettings);
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
                        Console.WriteLine("Error in SNMP reply. Error {0} index {1}",
                        result.Pdu.ErrorStatus,
                        result.Pdu.ErrorIndex);
                    }
                    else
                    {
                        for (int i = 0; i < result.Pdu.VbList.Count(); i++)
                        {
                            connection.SaveMonitorDataToDatabase(agent.AgentNr, result.Pdu.VbList[i].Oid.ToString(), result.Pdu.VbList[i].Value.ToString());
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No response recieved from SNMP Agent");
                }
                target.Close();
            }
            Console.ReadLine();
        }
    }
}