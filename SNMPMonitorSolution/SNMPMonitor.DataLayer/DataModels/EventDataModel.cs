using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.DataLayer
{
    public class EventDataModel
    {
        private readonly int _eventNr;
        private readonly string _exceptionType;
        private readonly string _category;
        private readonly DateTime _eventTimestamp;
        private readonly int _hResult;
        private readonly string _message;
        private readonly string _stacktrace;

        public EventDataModel(int eventNr, string exceptionType, string category, DateTime eventTimestamp, int hResult, string message, string stacktrace)
        {
            _eventNr = eventNr;
            _exceptionType = exceptionType;
            _category = category;
            _eventTimestamp = eventTimestamp;
            _hResult = hResult;
            _message = message;
            _stacktrace = stacktrace;
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
    }
}
