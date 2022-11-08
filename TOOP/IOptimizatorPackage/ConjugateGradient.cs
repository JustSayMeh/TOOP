using Functionals;
using Functions;
using Geometry;
using System;

namespace Optimizators
{
    class ConjugateGradient : IOptimizator
    {
        private double e = 1e-5;
        private double optimize(Func<double, double> func, double a, double b, double e)
        {
            do
            {
                double x1 = (a + b - e / 100) / 2;
                double x2 = (a + b + e / 100) / 2;
                double fx1 = func(x1);
                double fx2 = func(x2);
                if (fx1 < fx2)
                {
                    b = x2;
                }
                else
                {
                    a = x1;
                }
            } while (Math.Abs(a - b) > e);
            return (a + b) / 2;
        }

        private double calcLambda(Func<IVector, double> func, IVector x, IVector S)
        {
            return optimize((lambda) => func(x.Add(S.Multiple(lambda))), -10, 10, 1e-8);
        }

        public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters, IVector minimumParameters = null, IVector maximumParameters = null)
        {
            if (!(objective is IDifferentiableFunctional))
            {
                throw new ArgumentException();
            }
            var xk = initialParameters;
            var functional = objective as IDifferentiableFunctional;
            while (true)
            {
                int j = 0;
                var func = (IDifferentiableFunction)function.Bind(initialParameters);
                var S = functional.Gradient(func).Invers();
                var dfxkNormOld = S.Norm();
                do
                {
                    var lambda = calcLambda((IVector x) =>
                    {
                        var bindedFunction = function.Bind(x);
                        return functional.Value(bindedFunction);
                    }, xk, S);
                    var xk1 = xk.Add(S.Multiple(lambda));
                    if (xk1.Subtract(xk).Norm() < e)
                    {
                        return xk;
                    }
                    xk = xk1;
                    var bindedFunction = (IDifferentiableFunction)function.Bind(xk);
                    var fxk = functional.Value(bindedFunction);
                    var dfxk = functional.Gradient(bindedFunction);
                    var dfxkNorm = dfxk.Norm();
                    double w = (dfxkNorm * dfxkNorm) / (dfxkNormOld * dfxkNormOld);
                    dfxkNormOld = dfxkNorm;
                    S = dfxk.Invers().Add(S.Multiple(w));
                    if (j + 1 > initialParameters.Size)
                    {
                        break;
                    }
                    j += 1;
                    if (S.Norm() < e)
                    {
                        return xk;
                    }
                } while (true);
            }
        }
    }
}
