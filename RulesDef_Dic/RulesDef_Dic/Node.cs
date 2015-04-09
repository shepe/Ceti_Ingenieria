using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesDef_Dic
{
    class Node
    {
        public int Value;
        public int Level;
        public List<Node> LevelDependencies;

        public Node()
        {
            LevelDependencies = new List<Node>();
            Value = 0;
            Level = 0;
        }

        public Node(int v, int l)
        {
            LevelDependencies = new List<Node>();
            Value = v;
            Level = l;
        }
    }
}
