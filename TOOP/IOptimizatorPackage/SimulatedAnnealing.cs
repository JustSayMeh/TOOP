using Functionals;
using Functions;
using Geometry;
using System;

namespace Optimizators
{
    delegate IVector StateFunc(IVector S, double T);
    delegate double Temerature(int T);
    class SimulatedAnnealing : IOptimizator
    {
        private Temerature T;
        private StateFunc stateFunc;
        private double tMin;
        private Random random;
        public SimulatedAnnealing(Temerature Tdecrease, StateFunc stateFunc, double tMin)
        {
            T = Tdecrease;
            this.stateFunc = stateFunc;
            this.tMin = tMin;
            random = new Random();
        }

        private bool IsTransition(double dE, double t)
        {
            var P = Math.Exp(-dE / t);
            return random.NextDouble() <= P;
        }
        public IVector Minimize(IFunctional functional, IParametricFunction function, IVector initialParameters, IVector minimumParameters = null, IVector maximumParameters = null)
        {
            int i = 1;
            double t = T(i);
            var xk = initialParameters;
            var bindedFunction = function.Bind(xk);
            var fxk = functional.Value(bindedFunction);
            
            while (t > tMin)
            {
                var xkp1 = stateFunc(xk, t);
                bindedFunction = function.Bind(xkp1);
                var fxkp1 = functional.Value(bindedFunction);
                var dE = fxkp1 - fxk;

                if (IsTransition(dE, t))
                {
                    xk = xkp1;
                    fxk = fxkp1;
                }
                t = T(++i);
            }
            return xk;
        }
    }
}
