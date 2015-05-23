using Newtonsoft.Json.Linq;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.BusinessLayer
{
    public class SNMPWalk
    {
        private UdpTarget target;
        private Oid oid;

        public SNMPWalk(UdpTarget target, string oid){
            this.target = target;
            this.oid = new Oid(oid);
        }

        public JObject Walk()
        {
            JObject output = new JObject();
            JArray array = new JArray();

            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = SnmpVersion.Ver2;

            Oid lastOid = (Oid)this.oid.Clone();
            Pdu pdu = new Pdu(PduType.GetBulk);
            pdu.NonRepeaters = 0;
            while (lastOid != null)
            {
                if (pdu.RequestId != 0)
                {
                    pdu.RequestId += 1;
                }
                pdu.VbList.Clear();
                pdu.VbList.Add(lastOid);
                SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
                if (result != null)
                {
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        lastOid = null;
                        throw new SnmpErrorStatusException("Error in SNMP reply. Error " + result.Pdu.ErrorStatus + " index " + result.Pdu.ErrorIndex,result.Pdu.ErrorStatus, result.Pdu.ErrorIndex);
                            
                    }
                    else
                    {
                        foreach (Vb v in result.Pdu.VbList)
                        {
                            if (this.oid.IsRootOf(v.Oid))
                            {
                                JObject value = new JObject();
                                value.Add("OID", v.Oid.ToString());
                                value.Add("Type", SnmpConstants.GetTypeName(v.Value.Type));
                                value.Add("Value", v.Value.ToString());
                                array.Add(value);

                                if (v.Value.Type == SnmpConstants.SMI_ENDOFMIBVIEW)
                                    lastOid = null;
                                else
                                    lastOid = v.Oid;
                            }
                            else
                            {
                                lastOid = null;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    throw new SnmpNetworkException("No response received from SNMP agent.");
                }
            }

            output.Add("Results", array);

            return output;
        }
    }
}
