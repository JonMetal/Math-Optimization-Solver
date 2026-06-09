using MathOptimizationSolver.MatrixMathExtentions;
using MathOptimizationSolver.NewPlanGenerator;
using MathOptimizationSolver.PotentialMatrix;
using MathOptimizationSolver.ReferencePlanGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver
{
    public class Solver
    {
        private readonly INewPlanGenerator _newPlanGen;
        private readonly IPotentialMatrixGenerator _potentialMatrixGen;
        private readonly IReferencePlanGenerator _referencePlanGen;

        public Solver(INewPlanGenerator newPlanGenerator, IPotentialMatrixGenerator potentialMatrix, IReferencePlanGenerator referencePlanGenerator)
        {
            _newPlanGen = newPlanGenerator;
            _potentialMatrixGen = potentialMatrix;
            _referencePlanGen = referencePlanGenerator;
        }

        public int[,] SolveOptimization(NodeNet net)
        {
            Solve solve = _referencePlanGen.GetReferenceSolvePlan(net);
            int[,] potentialMatrix = _potentialMatrixGen.PotentialMatrixGet(solve, net);
            while (MatrixMath.FindMax(potentialMatrix, net.Dimension) > 0)
            {
                solve.Plan = _newPlanGen.NewPlanGet(potentialMatrix, net, solve);
                potentialMatrix = _potentialMatrixGen.PotentialMatrixGet(solve, net);
            }
            return solve.Plan;
        }
    }
}
