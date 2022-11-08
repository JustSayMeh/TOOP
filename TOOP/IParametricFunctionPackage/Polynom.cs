using Geometry;

namespace Functions
{
    class PolynomParametricFunction : IParametricFunction
    {
        public IFunction Bind(IVector parameters)
        {
            return new PolynomFunction(parameters);
        }
    }


    class PolynomFunction : IFunction
    {
        private IVector parameters;

        public PolynomFunction(IVector parameters)
        {
            this.parameters = parameters;
        }

        public double Value(IVector point)
        {
            int parametsSize = parameters.Size;
            double result = 0;
            double x = point[0]; 
            for (int i = parametsSize - 1; i > 0; i--)
            {
                result = (result + parameters[i]) * x;
            }
            return result + parameters[0];
        }
    }
}
