using Newtonsoft.Json.Linq;
using SNMPMonitor.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNMPMonitor.PresentationLayer.Models
{
    public class EventModel
    {
        private readonly int _eventNr;
        private readonly string _exceptionType;
        private readonly string _category;
        private readonly DateTime _eventTimestamp;
        private readonly int _hResult;
        private readonly string _message;
        private readonly string _stacktrace;

        public EventModel(Event eventObject)
        {
            _eventNr = eventObject.EventNr;
            _exceptionType = eventObject.ExceptionType;
            _category = eventObject.Category;
            _eventTimestamp = eventObject.EventTimestamp;
            _hResult = eventObject.HResult;
            _message = eventObject.Message;
            _stacktrace = eventObject.Stacktrace;
        }

        public int EventNr
        {
            get
            {
                return _eventNr;
            }
        }

        public string ExceptionType
        {
            get
            {
                return _exceptionType;
            }
        }

        public string Category
        {
            get
            {
                return _category;
            }
        }

        public DateTime EventTimestamp
        {
            get
            {
                return _eventTimestamp;
            }
        }

        public int HResult
        {
            get
            {
                return _hResult;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public string Stacktrace
        {
            get
            {
                return _stacktrace;
            }
        }

        public EventModel(JObject json)
        {
            JArray array = json.Descendants().OfType<JProperty>().First(x => x.Name == "param").Value.ToObject<JArray>();
            if (array != null)
            {
                JProperty eventNrProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "EventNr");
                JProperty exceptionTypeProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "ExceptionType");
                JProperty categoryProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "Category");
                JProperty eventTimestampProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "EventTimestamp");
                JProperty hResultProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "HResult");
                JProperty messageProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "Message");
                JProperty stacktraceProperty = array.Descendants().OfType<JProperty>().First(x => x.Name == "Stacktrace");

                this._exceptionType = exceptionTypeProperty.Value.ToString();
                this._category = categoryProperty.Value.ToString();
                this._message = messageProperty.Value.ToString();
                this._stacktrace = stacktraceProperty.Value.ToString();

                int eventNrParse = 0;
                if (int.TryParse(eventNrProperty.Value.ToString(), out eventNrParse))
                {
                    this._eventNr = eventNrParse;
                }
                else
                {
                    throw new FormatException("The JObject has to be in Format {param=[{'EventNr','<Value>'},{'ExceptionType','<value>'},{'Category','<value>'},{'EventTimestamp','<value>'},{'HResult','<value>'},{'Message','<value>'},{'Stacktrace','<value>'}]}");
                }

                DateTime eventTimestampParse = DateTime.MinValue;
                if (DateTime.TryParse(eventTimestampProperty.Value.ToString(), out eventTimestampParse))
                {
                    this._eventTimestamp = eventTimestampParse;
                }
                else
                {
                    throw new FormatException("The JObject has to be in Format {param=[{'EventNr','<Value>'},{'ExceptionType','<value>'},{'Category','<value>'},{'EventTimestamp','<value>'},{'HResult','<value>'},{'Message','<value>'},{'Stacktrace','<value>'}]}");
                }

                int hResultParse = 0;
                if (int.TryParse(hResultProperty.Value.ToString(), out hResultParse))
                {
                    this._hResult = hResultParse;
                }
                else
                {
                    throw new FormatException("The JObject has to be in Format {param=[{'EventNr','<Value>'},{'ExceptionType','<value>'},{'Category','<value>'},{'EventTimestamp','<value>'},{'HResult','<value>'},{'Message','<value>'},{'Stacktrace','<value>'}]}");
                }

            }
            else
            {
                throw new FormatException("The JObject has to be in Format {param=[{'EventNr','<Value>'},{'ExceptionType','<value>'},{'Category','<value>'},{'EventTimestamp','<value>'},{'HResult','<value>'},{'Message','<value>'},{'Stacktrace','<value>'}]}");
            }
        }
    }
}