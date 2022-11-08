using Geometry;
using System;

namespace Functions
{
    class SplineFunction : IParametricFunction
    {
        private double start;
        private double end;
        private double h;
        public SplineFunction(double start, double end, double h)
        {
            this.start = start;
            this.end = end;
            this.h = h;
        }
        public IFunction Bind(IVector parameters)
        {
            if ((end - start) / h > parameters.Size * 4)
            {
                throw new ArgumentException("not enough parameters!");
            }
            return new Spline(parameters, start, end, h);
        }
    }
    class Spline : IFunction
    {
        private IVector parameters;
        private double start;
        private double end;
        private double h;
        public Spline(IVector parameters, double start, double end, double h)
        {
            this.parameters = parameters;
            this.start = start;
            this.end = end;
            this.h = h;
        }
        public double Value(IVector point)
        {
            var x = point[0];
            if (x > end)
                return 0;
            int index = (int)((x - start) / h);
            double a = parameters[index * 4];
            double b = parameters[index * 4 + 1];
            double c = parameters[index * 4 + 2];
            double d = parameters[index * 4 + 3];
            double diff = x - (start + h * index);
            return b * diff + c * diff * diff + d * diff * diff * diff + a;
        }
    }
}
