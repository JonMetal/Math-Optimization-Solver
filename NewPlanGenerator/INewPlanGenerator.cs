using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver.NewPlanGenerator
{
    public interface INewPlanGenerator
    {
        public int[,] NewPlanGet(int[,] potentialMatrix, NodeNet net, Solve solve);
    }
}
