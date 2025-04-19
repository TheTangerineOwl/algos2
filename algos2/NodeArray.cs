using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algos2
{
    class NodeArray : INode
    {
        char symbol;
        //List<Node> branches = new();
        NodeArray?[] branches = new NodeArray?[58];
        // ascii - 65 
        // ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz

        bool isKey = false;

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
            /*foreach (NodeArray node in branches)
            {
                if (node.symbol == value)
                    return true;
            }*/
            if (value > 122 || value < 65)
                throw new ArgumentException($"Некорректный символ {value}!");
            return branches[value - 65] != null;
        }

        public INode AddChild(char value)
        {
            NodeArray child = new NodeArray();
            child.symbol = value;
            //branches.Add(child);
            if (!char.IsAscii(value) || value > 122 || value < 65)
                throw new ArgumentException($"Некорректный символ {value}!");
            branches[value - 65] = child;
            return child;
        }

        public INode? GetChild(char value)
        {
            /*foreach (NodeArray node in branches)
            {
                if (node.symbol == value)
                    return node;
            }*/
            if (value > 122 || value < 65)
                throw new ArgumentException($"Некорректный символ {value}!");
            return branches[value - 65];
        }

        public List<INode> GetDescendants()
        {
            List<INode> desc = new();
            foreach (NodeArray child in branches)
            {
                if (child != null)
                {
                    desc.Add(child);
                    desc.AddRange(child.GetDescendants());
                }
            }
            return desc;
        }

        public List<string> GetWords(string parentWord = "", int fromIndex = 0)
        {
            List<string> pref = new();
            if (IsKey)
                return [parentWord];
            /*foreach (NodeArray child in branches)
            {
                if (child == null)
                    continue;
                pref.AddRange(child.GetWords(parentWord + child.Value));
            }*/

            if (fromIndex < parentWord.Length)
            {
                char ch = parentWord[fromIndex];
                if (ch > 122 || ch < 65)
                    throw new ArgumentException($"Некорректный символ {ch}!");
                NodeArray? node = branches[ch - 65];
                if (node == null)
                    return [parentWord];
                pref.AddRange(node.GetWords(parentWord + node.Value, fromIndex + 1));
            }
            else
            {
                foreach (NodeArray branch in branches)
                {
                    if (branch != null)
                        pref.AddRange(branch.GetWords(parentWord + branch.Value, fromIndex + 1));
                }
            }

            return pref;
        }
    }
}
