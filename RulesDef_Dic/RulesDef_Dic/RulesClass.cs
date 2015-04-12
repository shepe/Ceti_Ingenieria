using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesDef_Dic
{
    class RulesClass
    {
        public int id;
        public List<int> list;

        public RulesClass(List<int> l, int ID)
        {
            id = ID;
            list = new List<int>();
            list = l;
        }
    }
}
