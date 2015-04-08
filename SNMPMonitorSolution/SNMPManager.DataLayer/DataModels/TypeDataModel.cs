using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class TypeDataModel
    {
        private int _typeNr;
        private String _name;

        public TypeDataModel(int typeNr, String name)
        {
            _typeNr = typeNr;
            _name = name;
        }
    }
}
