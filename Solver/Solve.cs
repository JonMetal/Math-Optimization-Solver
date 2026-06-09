using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver
{
    public class Solve
    {
        public List<int> ProviderValues { get; set; } = [];
        public List<int> ReceiverValues { get; set; } = [];

        public int[,] Plan { get; set; }

        public (int N, int M) Dimension { get; private set; }

        public Solve(NodeNet nodeNet)
        {
            ProviderValues.AddRange(nodeNet.Providers);
            ReceiverValues.AddRange(nodeNet.Receivers);
            Plan = new int[nodeNet.Dimension.N, nodeNet.Dimension.M];
            Dimension = nodeNet.Dimension;
        }

        public bool IsDone()
        {
            return ProviderValues.Max() <= 0 || ReceiverValues.Max() <= 0;
        }
    }
}
