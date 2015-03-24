using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer
{
    public class TypeModel
    {
        private int _typeNr;
        private String _name;

        public TypeModel(int TypeNr, String Name)
        {
            _typeNr = TypeNr;
            _name = Name;
        }
    }
}
