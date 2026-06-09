using System;
using System.Collections.Generic;
using System.Text;

namespace MathOptimizationSolver
{
    public struct NodeNet
    {
        public int[] Receivers { get; private set; }
        public int[] Providers { get; private set; }

        public const int FictRate = int.MaxValue;

        private readonly int[,] _rates;

        public NodeNet(int[] receivers, int[] providers, int[,] rates)
        {
            int receiversSum = receivers.Sum();
            int providersSum = providers.Sum();
            if (receiversSum != providersSum)
            {
                if (providersSum > receiversSum)
                {
                    Receivers = [.. receivers, providersSum - receiversSum];
                    Providers = [.. providers];
                }
                else
                {
                    Providers = [.. providers, receiversSum - providersSum];
                    Receivers = [.. receivers];
                }
                _rates = new int[Providers.Length, Receivers.Length];
                for (int i = 0; i < Providers.Length; i++)
                {
                    for (int j = 0; j < Receivers.Length; j++)
                    {
                        try
                        {
                            _rates[i, j] = rates[i, j];
                        }
                        catch
                        {
                            _rates[i, j] = FictRate - 1;
                        }
                    }
                }
            }
            else
            {
                Receivers = [.. receivers];
                Providers = [.. providers];
                _rates = rates.Clone() as int[,] ?? throw new NullReferenceException("Rates is not defined");
            }
        }

        public readonly (int N, int M) Dimension { get { return (Providers.Length, Receivers.Length); } }        

        public readonly int[,] Rates { get { return _rates; } }

        public readonly int GetRate(int p, int r)
        {
            try
            {
                return _rates[p, r];
            }
            catch
            {
                return FictRate;
            }
        }
    }
}
