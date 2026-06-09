using MathOptimizationSolver.MatrixMathExtentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver.NewPlanGenerator
{
    public class NewPlanGenerator : INewPlanGenerator
    {
        public int[,] NewPlanGet(int[,] potentialMatrix, NodeNet net, Solve solve)
        {
            (int indexMax1, int indexMax2) = MatrixMath.FindMaxIndex(potentialMatrix, net.Dimension);
            List<(int r, int c)> cycle = FindCycle(solve.Plan, (indexMax1, indexMax2));
            int valueMin = solve.Plan[cycle[1].r, cycle[1].c];
            int[,] result = solve.Plan.Clone() as int[,] ?? throw new NullReferenceException();
            for (int i = 1; i < cycle.Count; i += 2)
            {
                if (result[cycle[i].r, cycle[i].c] < valueMin)
                {
                    valueMin = result[cycle[i].r, cycle[i].c];
                }
            }
            for (int i = 1; i < cycle.Count; i += 2)
            {
                result[cycle[i].r, cycle[i].c] -= valueMin;
                result[cycle[i - 1].r, cycle[i - 1].c] += valueMin;
            }
            return result;
        }

        public static List<(int r, int c)> FindCycle(int[,] plan, (int r, int c) start)
        {
            int rows = plan.GetLength(0);
            int cols = plan.GetLength(1);

            List<(int r, int c)> path = new();
            HashSet<(int r, int c)> visited = [];

            if (!TryFindCycle(plan, start.r, start.c, start, path, visited, rows, cols, true))
                throw new Exception("Not find cycle");

            return path;
        }

        public static bool TryFindCycle(int[,] plan, int r, int c, (int r, int c) start, List<(int r, int c)> path, HashSet<(int r, int c)> visited, int rows, int cols, bool isColumn)
        {
            if (path.Count > 0 && r == start.r && c == start.c)
                return true;

            if (path.Count >= rows * cols)
                return false;

            var pos = (r, c);
            if (visited.Contains(pos))
                return false;
            visited.Add(pos);
            path.Add(pos);
            if(isColumn)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (j == c) continue;

                    bool isFilled = (r == start.r && j == start.c) ||
                                    (plan[r, j] != 0);

                    if (isFilled)
                    {
                        if (TryFindCycle(plan, r, j, start, path, visited, rows, cols, false))
                            return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    if (i == r) continue;

                    bool isFilled = (i == start.r && c == start.c) ||
                                    (plan[i, c] != 0);

                    if (isFilled)
                    {
                        if (TryFindCycle(plan, i, c, start, path, visited, rows, cols, true))
                            return true;
                    }
                }
            }

            path.RemoveAt(path.Count - 1);
            visited.Remove(pos);
            return false;
        }
    }
}
/* 
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
   [18, 19, 22, 25]  - fict price matrix
   [27, 28, 31, 34]

   {28, 18, 27, 24}
   {18, 27, 32, 21} - rates
   {27, 23, 31, 34}  

   { 0, 120, 0, 80  } 
   { 150, 0, 0, 0   } 
   { 40, 0, 110, 50 }

   [-11, 0, -6, 0]
   [0, -8, -10, 4] - potential matrix
   [0, 5, 0, 0   ]
*/