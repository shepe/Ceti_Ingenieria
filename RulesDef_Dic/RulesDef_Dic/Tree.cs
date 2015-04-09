﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesDef_Dic
{
    class Tree
    {
        public Node root;
        public List<Node> ContentTree;
        public List<int> review = new List<int>();
        public List<string> QueryList = new List<string>();
        public int ite = 0, cont = 0;

        public Tree()
        {
            root = null;
            ContentTree = new List<Node>();
        }

        public Tree(Node n)
        {
            root = n;
            ContentTree = new List<Node>();
        }

        public void GenTree(List<List<int>> listall, List<Node> Svalue, List<int> rev)
        {
            cont = 0;
            rev.Clear();
            //for (int i = ite + 1; i < ite + cont + 1; i++)
            for (int i = 0; i < ContentTree.Count; i++)
            {
                if (ContentTree[i].Level < Svalue[0].Level)
                {
                    if (!rev.Contains(Math.Abs(ContentTree[i].Value)))
                        rev.Add(Math.Abs(ContentTree[i].Value));
                    foreach (var item in ContentTree[i].LevelDependencies)
                    {
                        if (!rev.Contains(Math.Abs(item.Value)))
                            rev.Add(Math.Abs(item.Value));
                    }
                }
            }

            foreach (var sv in Svalue)
            {
                //Node n = new Node(sv);

                foreach (var item in listall)
                {
                    if (item.Contains(sv.Value) || item.Contains(sv.Value * -1))
                    {
                        foreach (var item2 in item)
                        {
                            if (item2 != sv.Value && item2 != sv.Value * -1)
                            {
                                if (!rev.Contains(Math.Abs(item2)))
                                {
                                    sv.LevelDependencies.Add(new Node(Math.Abs(item2), sv.Level + 1));
                                    //rev.Add(Math.Abs(item2));
                                }
                            }
                        }
                    }
                }

                ContentTree.Add(sv);
                cont++;
            }
            
            //cont = 0;
                try
                {
                    GenTree(listall, ContentTree.ElementAt(ite++).LevelDependencies, rev);
                }
                catch (Exception error)
                {
                    return;
                }
        }

        public void CutTree()
        {
            if (ContentTree.Count == 0)
                return;
            else
            {
                List<int> listquery = new List<int>();
                string query = "";
                List<Node> aux = new List<Node>();

                foreach (var item in ContentTree)
                {
                    foreach (var item2 in item.LevelDependencies)
                    {
                        if (item2.LevelDependencies.Count == 0)
                            if (!listquery.Contains(item2.Value))
                                listquery.Add(item2.Value);   
                    }

                    if (item.LevelDependencies.Count != 0)
                        aux.Add(item);
                    else
                        if (!listquery.Contains(item.Value))
                            listquery.Add(item.Value); 
                }

                foreach (var item in listquery)
                {
                    foreach (var n in aux)
                    {
                        for (int i = 0; i < n.LevelDependencies.Count; i++)
                        {
                            if (n.LevelDependencies[i].Value == item)
                                n.LevelDependencies.RemoveAt(i);
                        }
                    }
                    query += item.ToString() + " ";
                }

                QueryList.Add(query);

                ContentTree = aux;

                CutTree();
            }
        }
    }
}
