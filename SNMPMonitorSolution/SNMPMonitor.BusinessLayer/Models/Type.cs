using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.BusinessLayer
{
    public class Type
    {
        private readonly int _typeNr;
        private readonly string _name;

        public Type(int typeNr, string name)
        {
            _typeNr = typeNr;
            _name = name;
        }

        public int TypeNr 
        {
            get 
            {
                return _typeNr;
            }
        }

        public string Name
        {
            get 
            {
                return _name; 
            }
        }
    }
}
