using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class TypeDataModel
    {
        private readonly int _typeNr;
        private readonly String _name;

        public TypeDataModel(int typeNr, String name)
        {
            _typeNr = typeNr;
            _name = name;
        }
    }
}
