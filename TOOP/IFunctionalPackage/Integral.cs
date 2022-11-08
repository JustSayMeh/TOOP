using Functions;
using Geometry;

namespace Functionals
{
    class Integral : IFunctional
    {
        private readonly IVector a;
        private readonly IVector b;

        public Integral(IVector a, IVector b)
        {
            this.a = a;
            this.b = b;
        }
        public double Value(IFunction function)
        {
            var arg = VectorImpl.Of(new double[a.Size]);
            return componentIntegral(function, 0, arg);
        }

        private double componentIntegral(IFunction function, int i, IVector arg)
        {
            var argA = arg.Copy();
            var argB = arg.Copy();
            var argAB = arg.Copy();
            argA[i] = a[i];
            argB[i] = b[i];
            argAB[i] = (a[i] + b[i]) / 2;
            if (i == a.Size - 1)
            {
                return (b[i] - a[i]) / 6 * (function.Value(argA) + 4 * function.Value(argAB) + function.Value(argB));
            }
            return (b[i] - a[i]) / 6 * (componentIntegral(function, i + 1, argA) + 4 * componentIntegral(function, i + 1, argAB) + componentIntegral(function, i + 1, argB));

        }
    }
}
