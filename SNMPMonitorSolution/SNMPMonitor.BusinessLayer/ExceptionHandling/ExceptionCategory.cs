using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer.ExceptionHandling
{
    public enum ExceptionCategory
    {
        Normal,
        Low,
        Fatal,
        High
    }
}
