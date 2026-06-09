namespace MathOptimizationSolver.PotentialMatrix
{
    public interface IPotentialMatrixGenerator
    {
        public int[,] PotentialMatrixGet(Solve solve, NodeNet nodeNet);

        public int[,] FictPriceMatrixGet(Solve solve, NodeNet nodeNet);
    }
}
