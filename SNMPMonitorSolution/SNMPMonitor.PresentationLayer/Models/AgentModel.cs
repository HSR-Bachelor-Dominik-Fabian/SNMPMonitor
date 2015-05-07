using System;
using System.Collections.Generic;
using SNMPMonitor.BusinessLayer;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SNMPMonitor.PresentationLayer.Models
{
    public class AgentModel
    {
        private int _agentNr;
        private string _name;
        private string _ipAddress;
        private int _status;
        private int _port;
        private string _sysDesc;
        private string _sysName;
        private  string _sysUptime;

        public int AgentNr
        {
            get { return _agentNr; }
            set { _agentNr = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public string SysDesc
        {
            get { return _sysDesc; }
            set { _sysDesc = value; }
        }

        public string SysName
        {
            get { return _sysName; }
            set { _sysName = value; }
        }

        public string SysUptime
        {
            get { return _sysUptime; }
            set { _sysUptime = value; }
        }

        public AgentModel()
        {
            this._agentNr = 0;
            this._name = string.Empty;
            this._ipAddress = string.Empty;
            this._status = 0;
            this._port = 0;
            this._sysDesc = string.Empty;
            this._sysName = string.Empty;
            this._sysUptime = string.Empty;
        }

        public AgentModel(Agent model)
        {
            this._agentNr = model.AgentNr;
            this._name = model.Name;
            this._ipAddress = model.IPAddress;
            this._status = model.Status;
            this._port = model.Port;
            this._sysDesc = model.SysDesc;
            this._sysName = model.SysName;
            this._sysUptime = model.SysUptime;
        }

        public AgentModel(JObject json)
        {
            JArray array = json.Descendants().OfType<JProperty>().First(x => x.Name == "param").Value.ToObject<JArray>();
            if (array != null)
            {
                JProperty agentNrProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "AgentNr");
                JProperty nameProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "Name");
                JProperty ipAddressProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "IPAddress");
                JProperty statusProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "Status");
                JProperty portProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "Port");
                JProperty sysDescProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "sysDesc");
                JProperty sysNameProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "sysName");
                JProperty sysUptimeProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "sysUptime");

                this._name = nameProperty.Value.ToString();
                this._ipAddress = ipAddressProperty.Value.ToString();
                this._sysDesc = sysDescProperty.Value.ToString();
                this._sysName = sysNameProperty.Value.ToString();
                this._sysUptime = sysUptimeProperty.Value.ToString();

                int agentNrParse = 0;
                if (int.TryParse(agentNrProperty.Value.ToString(), out agentNrParse))
                {
                    this._agentNr = agentNrParse;
                }
                else
                {
                    throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'Name','<value>'},{'IPAddress','<value>'},{'Status','<value>'},{'Port','<value>'},{'sysDesc','<value>'},{'sysName','<value>'},{'sysUptime','<value>'}]}");
                }

                int statusParse = 0;
                if (int.TryParse(statusProperty.Value.ToString(), out statusParse))
                {
                    this._status = statusParse;
                }
                else
                {
                    throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'Name','<value>'},{'IPAddress','<value>'},{'Status','<value>'},{'Port','<value>'},{'sysDesc','<value>'},{'sysName','<value>'},{'sysUptime','<value>'}]}");
                }

                int portParse = 0;
                if (int.TryParse(portProperty.Value.ToString(), out portParse))
                {
                    this._port = portParse;
                }
                else
                {
                    throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'Name','<value>'},{'IPAddress','<value>'},{'Status','<value>'},{'Port','<value>'},{'sysDesc','<value>'},{'sysName','<value>'},{'sysUptime','<value>'}]}");
                }

            }
            else
            {
                throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'Name','<value>'},{'IPAddress','<value>'},{'Status','<value>'},{'Port','<value>'},{'sysDesc','<value>'},{'sysName','<value>'},{'sysUptime','<value>'}]}");
            }
        }
    }
}