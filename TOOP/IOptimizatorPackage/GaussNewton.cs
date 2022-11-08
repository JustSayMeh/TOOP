using Functionals;
using Functions;
using Geometry;
using System;

namespace Optimizators
{
    class GaussNewton : IOptimizator
    {
        public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters, IVector minimumParameters = null, IVector maximumParameters = null)
        {
            if (!(objective is ILeastSquaresFunctional))
            {
                throw new ArgumentException();
            }
            var functional = objective as ILeastSquaresFunctional;
            var xk = initialParameters;
            var xkPrev = initialParameters;
            do
            {
                var func = function.Bind(xk);
                var J = functional.Jacobian((IDifferentiableFunction) func);
                var F = functional.Residual(func);
                var JT = J.Transpose();
                xkPrev = xk;
                xk = xk.Subtract(JT.Multiple(J).Inverse().Multiple(JT.Multiple(F)));
            } while (xk.Subtract(xkPrev).Norm() > 1e-5);
            return xk;
        }
    }
}
