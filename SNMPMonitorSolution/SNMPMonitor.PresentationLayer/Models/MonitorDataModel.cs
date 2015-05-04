using Newtonsoft.Json.Linq;
using SNMPMonitor.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNMPMonitor.PresentationLayer.Models
{
    public class MonitorDataModel
    {
        private DateTime _monitorTimestamp;
        public DateTime MonitorTimestamp
        {
            get { return _monitorTimestamp; }
            set { _monitorTimestamp = value; }
        }
        private string _result;

        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        private int _agentID;

        public int AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }

        private string _objectID;

        public string ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        public MonitorDataModel()
        {
            this._result = string.Empty;
            this._agentID = 0;
            this._monitorTimestamp = new DateTime();
            this._objectID = string.Empty;
        }

        public MonitorDataModel(MonitorData model)
        {
            this._result = model.Result;
            this._objectID = model.ObjectID;
            this._monitorTimestamp = model.Timestamp;
            this._agentID = model.AgentNr;
        }

        public MonitorDataModel(JObject json)
        {
            try
            {
                JArray array = json.Descendants().OfType<JProperty>().First(x => x.Name == "param").Value.ToObject<JArray>();
                if (array != null)
                {
                    JProperty agentNrProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "AgentNr");
                    JProperty monitorTimestampProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "MonitorTimestamp");
                    JProperty objectIDProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "ObjectID");
                    JProperty resultProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "Result");

                    this._result = resultProperty.Value.ToString();
                    this._objectID = objectIDProperty.Value.ToString();
                    int agentNr = 0;
                    if (int.TryParse(agentNrProperty.Value.ToString(), out agentNr))
                    {
                        this._agentID = agentNr;
                    }
                    else
                    {
                        throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'MonitorTimestamp','<value>'},{'ObjectID','<value>'},{'Result','<value>'}]}");
                    }
                    DateTime monitorTimestamp;
                    if (DateTime.TryParse(monitorTimestampProperty.Value.ToString(), out monitorTimestamp))
                    {
                        this._monitorTimestamp = monitorTimestamp;
                    }
                    else
                    {
                        throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'MonitorTimestamp','<value>'},{'ObjectID','<value>'},{'Result','<value>'}]}");
                    }

                }
                else
                {
                    throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'MonitorTimestamp','<value>'},{'ObjectID','<value>'},{'Result','<value>'}]}");
                }
            }
            catch(Exception exc)
            {
                //throw new FormatException("The JObject has to be in Format {param=[{'AgentNr','<Value>'},{'MonitorTimestamp','<value>'},{'ObjectID','<value>'},{'Result','<value>'}]}\nException thrown: " + exc.Message);
                BusinessLayer.ExceptionHandling.ExceptionCore.HandleException(BusinessLayer.ExceptionHandling.ExceptionCategory.High, exc);
            }
        }
    }
}