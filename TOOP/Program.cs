using Functionals;
using Geometry;
using Optimizators;
using System;
using System.Collections.Generic;
using static Functions.Linear;

namespace TOOP
{
    class Program
    {
        static void Main(string[] args)
        {
            GaussTest();
            SimulatedAnnealingTest();
            ConjugateGradientTest();
        }

        static void GaussTest()
        {
            List<(IVector, double)> list = new();
            list.Add((VectorImpl.Of(1), 5));
            list.Add((VectorImpl.Of(3), 9));
            list.Add((VectorImpl.Of(4), 11));
            GaussNewton alg = new GaussNewton();
            var min = alg.Minimize(new L2Norm(list), new LinearParametricFunction(), VectorImpl.Of(-15, 10));
            Console.WriteLine(min);
        }


        static void SimulatedAnnealingTest()
        {
            List<(IVector, double)> list = new();
            list.Add((VectorImpl.Of(1), 5));
            list.Add((VectorImpl.Of(3), 9));
            list.Add((VectorImpl.Of(4), 11));
            Random random = new Random();
            SimulatedAnnealing alg = new SimulatedAnnealing((i) => 10.0 / i, (s, t) => {
                int arrSize = s.Size;
                var arr = new double[arrSize];
                for (int i = 0; i < arrSize; i++)
                {
                    arr[i] = random.NextDouble() * 2 - 1;
                }
                var dS = VectorImpl.Of(arr);
                var dsNorm = dS.Norm();
                return s.Add(dS.Multiple(t / dsNorm));
            }, 1e-7);
            var min = alg.Minimize(new L2Norm(list), new LinearParametricFunction(), VectorImpl.Of(-15, 10));
            Console.WriteLine(min);
        }

        static void ConjugateGradientTest()
        {
            List<(IVector, double)> list = new();
            list.Add((VectorImpl.Of(1), 5));
            list.Add((VectorImpl.Of(3), 9));
            list.Add((VectorImpl.Of(4), 11));
            ConjugateGradient alg = new ConjugateGradient();
            Random random = new Random();
            var min = alg.Minimize(new L2Norm(list), new LinearParametricFunction(), VectorImpl.Of(-15, 10));
            Console.WriteLine(min);
        }
    }


}
