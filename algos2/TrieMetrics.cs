using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algos2
{
    public static class TrieMetrics
    {
        public static int TotalNodesInSubtree(INode node)
        {
            int count = 1; // текущий узел
            foreach (var child in node.Branches)
                if (child != null)
                    count += TotalNodesInSubtree(child);
            return count;
        }

        public static int LeafNodesCount(INode node)
        {
            if (!node.Branches.Any()) return 1;

            int count = 0;
            foreach (var child in node.Branches)
                if (child != null)
                    count += LeafNodesCount(child);
            return count;
        }

        public static int InternalNodesCount(INode node, bool isRoot = true)
        {
            if (!node.Branches.Any())
                return 0; // Лист не считается внутренним

            int count = isRoot ? 0 : 1; // Не считаем корень
            foreach (var child in node.Branches)
                if (child != null)
                    count += InternalNodesCount(child, false); // передаём дальше, что это не корень
            return count;
        }

        public static int BranchingNodesCount(INode node)
        {
            int count = node.Branches.Count() > 1 ? 1 : 0;
            foreach (var child in node.Branches)
                if (child != null)
                    count += BranchingNodesCount(child);
            return count;
        }

        public static double AvgBranchingFactor(INode node)
        {
            int branchingNodes = BranchingNodesCount(node);
            if (branchingNodes == 0) return 0;

            double totalBranches = CountBranchesInBranchingNodes(node);
            return totalBranches / branchingNodes;
        }

        private static double CountBranchesInBranchingNodes(INode node)
        {
            double count = 0;

            int branchCount = node.Branches.Count();
            if (branchCount > 1)
                count += branchCount;

            foreach (var child in node.Branches)
                if (child != null)
                    count += CountBranchesInBranchingNodes(child);

            return count;
        }
    }
}
