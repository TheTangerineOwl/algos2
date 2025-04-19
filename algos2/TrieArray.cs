using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algos2
{
    public interface INode
    {
        public bool HasChild(char value);
        public INode AddChild(char value);

        public INode? GetChild(char value);
    }
}
