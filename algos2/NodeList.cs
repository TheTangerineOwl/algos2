using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace algos2
{
    public class NodeList : INode
    {
        char symbol;
        List<NodeList> branches = new();
        bool isKey = false;

        public IEnumerable<INode> Branches
        {
            get => branches.AsEnumerable();
        }

        public char Value
        {
            get { return symbol; }
            set
            {
                symbol = value;
            }
        }
        public bool IsKey
        {
            get { return isKey; }
            set
            {
                isKey = value;
            }
        }

        public bool HasChild(char value)
        {
            foreach (NodeList NodeList in branches)
            {
                if (NodeList.symbol == value)
                    return true;
            }
            return false;
        }

        public INode AddChild(char value)
        {
            NodeList child = new NodeList();
            child.symbol = value;
            branches.Add(child);
            return child;
        }

        public INode? GetChild(char value)
        {
            foreach (NodeList NodeList in branches)
            {
                if (NodeList.symbol == value)
                    return NodeList;
            }
            return null;
        }

        public List<INode> GetDescendants()
        {
            List<INode> desc = new();
            foreach (NodeList child in branches)
            {
                desc.Add(child);
                desc.AddRange(child.GetDescendants());
            }
            return desc;
        }

        public List<string> GetWords(string parentWord = "", int fromIndex = 0)
        {
            List<string> pref = new();
            if (branches.Count == 0 && IsKey)
                return [parentWord];
            foreach (NodeList child in branches)
                pref.AddRange(child.GetWords(parentWord + child.Value));
            return pref;
        }
    }
}
