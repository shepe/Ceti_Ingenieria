using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesDef_Dic
{
    class PropValue
    {
        public int Prop;
        public int Value;
        public string Who;

        public PropValue(int p)
        {
            Prop = p;
            Value = 0;
            Who = "none";
        }

        public void AskForValue(Boolean v)
        {
            Value = (v == true)? 1 : -1;
        }
    }
}
