using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algos2
{
    class NodeArray : INode
    {
        char symbol;
        NodeArray?[] branches = new NodeArray?[59];
        // ascii - 64, A=65 индекс 1
        // ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz, $ - конечный симв с индексом 0

        public IEnumerable<INode?> Branches
        {
            //get => branches.AsEnumerable();

            get => branches.Where(x => x != null);
        }

        public char Value
        {
            get { return symbol; }
            set
            {
                symbol = value;
            }
        }

        public bool HasChild(char value)
        {
            if (value == '$')
                return branches[0] != null;
            if (value > 122 || value < 65)
                throw new ArgumentException($"Некорректный символ {value}!");
            return branches[value - 64] != null;
        }

        public INode? AddChild(char value)
        {
            NodeArray child = new NodeArray();
            child.symbol = value;
            if (Value == '$')
                return null;
            if (value == '$')
            {
                branches[0] = child;
                return child;
            }
            if (value > 122 || value < 65)
                throw new ArgumentException($"Некорректный символ {value}!");
            branches[value - 64] = child;
            return child;
        }

        public INode? GetChild(char value)
        {
            if (Value == '$')
                return null;
            if (value == '$')
                return branches[0];
            if (value > 122 || value < 65)
                throw new ArgumentException($"Некорректный символ {value}!");
            return branches[value - 64];
        }

        public List<INode> GetDescendants()
        {
            List<INode> desc = new();
            if (Value == '$')
                return desc;
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
            if (Value == '$')
                return [parentWord.Remove(parentWord.Length - 1)];


            if (fromIndex < parentWord.Length)
            {
                char ch = parentWord[fromIndex];
                if ((ch > 122 || ch < 65) && ch != '$')
                    throw new ArgumentException($"Некорректный символ {ch}!");
                NodeArray? node;
                if (ch == '$')
                {
                    node = branches[0];
                    return [parentWord.Remove(parentWord.Length - 1)];
                }
                else
                    node = branches[ch - 64];
                if (node == null)
                    return [parentWord];
                pref.AddRange(node.GetWords(parentWord + node.Value, fromIndex + 1));
            }
            else
            {
                foreach (NodeArray branch in branches)
                    if (branch != null)
                        pref.AddRange(branch.GetWords(parentWord + branch.Value, fromIndex + 1));
            }

            return pref;
        }
    }
}
