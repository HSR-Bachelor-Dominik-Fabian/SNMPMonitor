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
        private static String _iPAdresse = "152.96.56.75";

        static void Main(string[] args)
        {
            OctetString community = new OctetString("public");

            AgentParameters param = new AgentParameters(community);

            param.Version = SnmpVersion.Ver1;
            IpAddress agent = new IpAddress(_iPAdresse);
            UdpTarget target = new UdpTarget((IPAddress)agent, 40001, 2000, 1);
 
            Pdu pdu = new Pdu(PduType.Get);
            pdu.VbList.Add("1.3.6.1.2.1.1.1.0"); //sysDescr
            pdu.VbList.Add("1.3.6.1.2.1.1.2.0"); //sysObjectID
            pdu.VbList.Add("1.3.6.1.2.1.1.3.0"); //sysUpTime
            pdu.VbList.Add("1.3.6.1.2.1.1.4.0"); //sysContact
            pdu.VbList.Add("1.3.6.1.2.1.1.5.0"); //sysName

            SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);
            DatabaseConnection connection = new DatabaseConnection("152.96.56.75", 40003, "Manager", "HSR-00228866");

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
                        connection.insertValueToDatabase(_iPAdresse, result.Pdu.VbList[i].Oid.ToString(), result.Pdu.VbList[i].Value.ToString());

                    }
                }
            }
            else
            {
                Console.WriteLine("No response received from SNMP agent.");
            }
            connection.getValuesFromDatacase();
            Console.ReadLine();
            target.Close();
        }
    }
}