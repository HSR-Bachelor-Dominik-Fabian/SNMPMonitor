using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPMonitor.DataLayer
{
    public class TypeModel
    {
        private int _typeNr;
        private String _name;

        public TypeModel(int typeNr, String name)
        {
            _typeNr = typeNr;
            _name = name;
        }
    }
}
