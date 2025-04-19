using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algos2
{
    public interface INode
    {
        public IEnumerable<INode?> Branches { get; }


        public char Value { get; set; }
        public bool IsKey { get; set; }

        public bool HasChild(char value);
        public INode AddChild(char value);

        public INode? GetChild(char value);

        public List<INode> GetDescendants();

        public List<string> GetWords(string parentWord = "", int fromIndex = 0);
    }
}
