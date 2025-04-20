using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algos2
{
    public class TrieMetrics
    {
        public int TotalNodes { get; private set; }
        public int TotalWords { get; private set; }
        public int InternalNodes { get; private set; }
        public int BranchingNodes { get; private set; }
        public int BranchesInBranching { get; private set; }
        public double AvgBranching { get; private set; }

        public void CountStats(INode root, bool isRoot)
        {
            TotalNodes = -1; // без root
            TotalWords = 0;
            InternalNodes = 0;
            BranchingNodes = 0;
            BranchesInBranching = 0;

            AllAtOnceRec(root, isRoot);

            InternalNodes = TotalNodes - TotalWords;
        }

        private void AllAtOnceRec(INode node, bool isRoot)
        {
            TotalNodes += 1;
            int branchCount = node.Branches.Count();
            if (branchCount == 0)
                TotalWords += 1;
            //else if (branchCount == 1)
                //InternalNodes = isRoot ? InternalNodes : InternalNodes + 1;
            else if (!isRoot)
            {
                InternalNodes += 1;
                if (branchCount > 1)
                {
                    BranchingNodes += 1;
                    BranchesInBranching += branchCount;
                    AvgBranching = (double)BranchesInBranching / (double)BranchingNodes;
                }
            }

            foreach (var child in node.Branches)
                if (child != null)
                    AllAtOnceRec(child, false);
        }
    }
}
