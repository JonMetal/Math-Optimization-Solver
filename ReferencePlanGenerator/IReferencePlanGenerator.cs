using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver.ReferencePlanGenerator
{
    public interface IReferencePlanGenerator
    {
        public Solve GetReferenceSolvePlan(NodeNet net);
    }
}
