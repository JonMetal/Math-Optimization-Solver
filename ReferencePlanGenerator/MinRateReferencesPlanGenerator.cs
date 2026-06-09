using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver.ReferencePlanGenerator
{
    public class MinRateReferencesPlanGenerator : IReferencePlanGenerator
    {
        public Solve GetReferenceSolvePlan(NodeNet net)
        {
            (int n, int m) = net.Dimension;
            Solve baseSolve = new(net);
            while (!baseSolve.IsDone())
            {
                (int index1, int index2) = FindMinRate(net, baseSolve);
                baseSolve.Plan[index1, index2] = baseSolve.ProviderValues[index1] < baseSolve.ReceiverValues[index2]
                    ? baseSolve.ProviderValues[index1] : baseSolve.ReceiverValues[index2];
                baseSolve.ProviderValues[index1] -= baseSolve.Plan[index1, index2];
                baseSolve.ReceiverValues[index2] -= baseSolve.Plan[index1, index2];
            }
            return baseSolve;
        }

        public static (int, int) FindMinRate(NodeNet net, Solve solve)
        {
            int index1 = -1, index2 = -1;
            (int n, int m) = net.Dimension;
            for (int i = 0; i < n; i++)
            {
                if (solve.ProviderValues[i] <= 0)
                {
                    continue;
                }
                for (int j = 0; j < m; j++)
                {
                    if (solve.ReceiverValues[j] <= 0)
                    {
                        continue;
                    }
                    if (net.GetRate(i, j) < net.GetRate(index1, index2))
                    {
                        (index1, index2) = (i, j);
                    }
                }
            }
            return (index1, index2);
        }
    }
}
