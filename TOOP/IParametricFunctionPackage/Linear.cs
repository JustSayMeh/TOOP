using Geometry;
using System;

namespace Functions
{
    class Linear
    {
        public class LinearParametricFunction : IParametricFunction
        {
            public IFunction Bind(IVector parameters)
            {
                return new LinearFunction(parameters);
            }
        }

        public class LinearFunction : IDifferentiableFunction
        {
            private IVector parameters;

            public LinearFunction(IVector parameters)
            {
                this.parameters = parameters;
            }

            public IVector Gradient(IVector point)
            {
                double[] gradient = new double[parameters.Size];
                Array.Copy(point.Elements, gradient, point.Size);
                gradient[parameters.Size - 1] = 1;
                return new VectorImpl(gradient);
            }

            public double Value(IVector point)
            {
                int parametsSize = parameters.Size;
                if (point.Size + 1 != parametsSize)
                {
                    throw new ArgumentException("Point size unacceptable");
                }
                double result = parameters[parametsSize - 1];
                for (int i = 0; i < point.Size; i++)
                {
                    result += point[i] * parameters[i];
                }
                return result;
            }
        }
    }
}
