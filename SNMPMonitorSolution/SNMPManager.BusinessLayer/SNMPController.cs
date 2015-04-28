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
using System.Data.SqlClient;
using SNMPManager.BusinessLayer.ExceptionHandling;

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
            try
            {
                DatabaseConnectionManager connection = new DatabaseConnectionManager(_connectionString);
                List<AgentDataModel> AgentList = connection.GetAgentsFromDatabase();

                Parallel.ForEach(AgentList, agent => GetSNMPDataFromSingleAgent(agent));
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
        }

        private void GetSNMPDataFromSingleAgent(AgentDataModel agent)
        {
            try
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
            catch (SnmpException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
        }

        private Pdu GetPduForAgent(AgentDataModel agent, DatabaseConnectionManager connection)       
        {
            Pdu pdu = new Pdu(PduType.Get);
            try
            {
                List<MonitoringTypeDataModel> monitoringTypeList = connection.GetMonitoringTypesForAgentForCheckFromDatabase(agent.AgentNr);   


                foreach (MonitoringTypeDataModel MonitoringType in monitoringTypeList)
                {
                    pdu.VbList.Add(MonitoringType.ObjectID);
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
            return pdu;
        }

        private void SendDataToDataLayer(AgentDataModel agent, SnmpV2Packet result, DatabaseConnectionManager connection)
        {
            try
            {
                List<KeyValuePair<string, string>> resultList = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < result.Pdu.VbList.Count(); i++)
                {
                    resultList.Add(new KeyValuePair<string, string>(result.Pdu.VbList[i].Oid.ToString(), result.Pdu.VbList[i].Value.ToString()));
                }
                connection.AddMonitorDataToDatabase(agent, resultList);
            }
            catch(SqlException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Fatal, e);
            }
            catch (NullReferenceException e)
            {
                ExceptionCore.HandleException(ExceptionCategory.High, e);
            }
            catch (Exception e)
            {
                ExceptionCore.HandleException(ExceptionCategory.Normal, e);
            }
        }
    }
}