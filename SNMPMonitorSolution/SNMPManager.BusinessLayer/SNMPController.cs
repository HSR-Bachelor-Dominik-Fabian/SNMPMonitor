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
using Newtonsoft.Json.Linq;
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
                try
                {
                    this.WalkThroughOid(target, connection, agent);
                }
                finally
                {
                    target.Close();
                }
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

        private void WalkThroughOid(UdpTarget target, DatabaseConnectionManager connection, AgentDataModel agent)
        {
            List<MonitoringTypeDataModel> MonitoringTypeList = connection.GetMonitoringTypesForAgentForCheckFromDatabase(agent.AgentNr);
            if (MonitoringTypeList.Count > 0) {
                List<KeyValuePair<string, string>> resultList = new List<KeyValuePair<string, string>>();
                foreach (MonitoringTypeDataModel type in MonitoringTypeList)
                {
                    SNMPWalk walk = new SNMPWalk(target, type.ObjectID);
                    JObject results = walk.Walk();
                    if (type.ObjectID.Equals("1.3.6.1.2.1.1.1") || type.ObjectID.Equals("1.3.6.1.2.1.1.5") || type.ObjectID.Equals("1.3.6.1.2.1.25.1.1"))
                    {
                        if ((results["Results"]).Count() > 0)
                        {
                            resultList.Add(new KeyValuePair<string, string>(type.ObjectID, ((results["Results"])[0])["Value"].ToString()));
                        }
                    }
                    else
                    {
                        resultList.Add(new KeyValuePair<string, string>(type.ObjectID, results.ToString()));
                    }
                }
                connection.AddMonitorDataToDatabase(agent, resultList);
            }
        }
    }
}