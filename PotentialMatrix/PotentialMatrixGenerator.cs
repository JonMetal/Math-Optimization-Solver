using MathOptimizationSolver.MatrixMathExtentions;
namespace MathOptimizationSolver.PotentialMatrix
{
    public class PotentialMatrixGenerator : IPotentialMatrixGenerator
    {


        public int[,] PotentialMatrixGet(Solve solve, NodeNet net)
        {
            int[,] fictPriceMatrix = FictPriceMatrixGet(solve, net);
            int[,] potentialMatrix = MatrixMath.MatrixSubstract(fictPriceMatrix, net.Rates, net.Dimension);
            return potentialMatrix;
        }

        public int[,] FictPriceMatrixGet(Solve solve, NodeNet net)
        {
            (int n, int m) = solve.Dimension;
            int[] valuesA = new int[n];
            int[] valuesB = new int[m];
            const int UNDEF = int.MaxValue;
            for (int i = 0; i < n; i++) valuesA[i] = UNDEF;
            for (int j = 0; j < m; j++) valuesB[j] = UNDEF;

            var visitedA = new bool[n];
            var visitedB = new bool[m];
            var queueA = new Queue<int>();
            var queueB = new Queue<int>();

            for (int startI = 0; startI < n; startI++)
            {
                if (visitedA[startI]) continue;
                bool hasBase = false;
                for (int j = 0; j < m; j++) if (solve.Plan[startI, j] != 0) { hasBase = true; break; }
                if (!hasBase) continue;

                valuesA[startI] = 0;
                visitedA[startI] = true;
                queueA.Enqueue(startI);

                while (queueA.Count > 0 || queueB.Count > 0)
                {
                    while (queueA.Count > 0)
                    {
                        int i = queueA.Dequeue();
                        for (int j = 0; j < m; j++)
                        {
                            if (solve.Plan[i, j] == 0) continue;
                            if (!visitedB[j])
                            {
                                valuesB[j] = net.GetRate(i, j) - valuesA[i];
                                visitedB[j] = true;
                                queueB.Enqueue(j);
                            }
                        }
                    }

                    while (queueB.Count > 0)
                    {
                        int j = queueB.Dequeue();
                        for (int i = 0; i < n; i++)
                        {
                            if (solve.Plan[i, j] == 0) continue;
                            if (!visitedA[i])
                            {
                                valuesA[i] = net.GetRate(i, j) - valuesB[j];
                                visitedA[i] = true;
                                queueA.Enqueue(i);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++) if (!visitedA[i]) valuesA[i] = 0;
            for (int j = 0; j < m; j++) if (!visitedB[j]) valuesB[j] = 0;

            int[,] result = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    result[i, j] = valuesA[i] + valuesB[j];

            return result;
        }
    }
}
/* 
    int[,] rates = {
        {28, 18, 27, 24},
        {18, 27, 32, 21},
        {27, 23, 31, 34}
    };

    int[,] minHandPlan = {
        { 0, 120, 0, 80 },
        { 150, 0, 0, 0 },
        { 40, 0, 110, 50 }
    };
   a1 + b2 = d12 = 18
   a1 + b4 = d14 = 24
   a2 + b1 = d21 = 18
   a3 + b1 = d31 = 27
   a3 + b3 = d33 = 31
   a3 + b4 = d34 = 34
   a1 = 0
   b2 = 18, b4 = 24, a3 = 10, b3 = 21, b1 = 17, a2 = 1
   a1 = 0, a2 = 1, a3 = 10
   b1 = 17, b2 = 18, b3 = 21, b4 = 24
   [17, 18, 21, 24]
   [18, 19, 22, 25] 
   [27, 28, 31, 34]

   {28, 18, 27, 24}
   {18, 27, 32, 21}
   {27, 23, 31, 34}  

   [-11, 0, -6, 0]
   [0, -8, -10, 4]
   [0, 5, 0, 0   ]
*/