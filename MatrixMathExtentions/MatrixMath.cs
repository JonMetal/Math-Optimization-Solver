using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver.MatrixMathExtentions
{
    public static class MatrixMath
    {
        public static int[,] MatrixSubstract(int[,] matrixA, int[,]matrixB, (int n, int m) dimension)
        {
            int[,] result = new int[dimension.n, dimension.m];
            for (int i = 0; i < dimension.n; i++)
            {
                for(int j = 0; j < dimension.m; j++)
                {
                    result[i, j] = matrixA[i, j] - matrixB[i, j];
                }
            }
            return result;
        }

        public static int[,] MatrixSum(int[,] matrixA, int[,] matrixB, (int n, int m) dimension)
        {
            int[,] result = new int[dimension.n, dimension.m];
            for (int i = 0; i < dimension.n; i++)
            {
                for (int j = 0; j < dimension.m; j++)
                {
                    result[i, j] = matrixA[i, j] + matrixB[i, j];
                }
            }
            return result;
        }

        public static (int, int) FindMaxIndex(int[,] array, (int n, int m) dimension)
        {
            int index1 = 0, index2 = 0;
            for (int i = 0; i < dimension.n; i++)
            {
                for (int j = 0; j < dimension.m; j++)
                {
                    if (array[i, j] > array[index1, index2])
                    {
                        index1 = i;
                        index2 = j;
                    }
                }
            }
            return (index1, index2);
        }
        public static int FindMax(int[,] array, (int n, int m) dimension)
        {
            (int index1, int index2) = FindMaxIndex(array, dimension);
            return array[index1, index2];
        }

        public static (int, int) FindMinIndex(int[,] array, (int n, int m) dimension)
        {
            int index1 = 0, index2 = 0;
            for (int i = 0; i < dimension.n; i++)
            {
                for (int j = 0; j < dimension.m; j++)
                {
                    if (array[i, j] < array[index1, index2])
                    {
                        index1 = i;
                        index2 = j;
                    }
                }
            }
            return (index1, index2);
        }

        public static int FindMin(int[,] array, (int n, int m) dimension)
        {
            (int index1, int index2) = FindMinIndex(array, dimension);
            return array[index1, index2];
        }
    }
}
