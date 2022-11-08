using Functions;
using Geometry;
using System;
using System.Collections.Generic;

namespace Functionals
{
    class L1Norm : IDifferentiableFunctional
    {
        private List<(IVector, double)> points;
        public L1Norm(List<(IVector, double)> points)
        {
            this.points = points;
        }
        public IVector Gradient(IDifferentiableFunction function)
        {
            double[] result = null;
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var pointVector = point.Item1;
                var pointValue = point.Item2;
                var gradient = function.Gradient(pointVector);
                if (result == null)
                    result = new double[gradient.Size];
                for (int j = 0; j < gradient.Size; j++)
                {
                    result[j] += -1 * Math.Sign(pointValue - function.Value(pointVector)) * gradient[j];
                }
            }
            return new VectorImpl(result);
        }

        public double Value(IFunction function)
        {
            double norm = 0;
            foreach (var point in points)
            {
                var pointVector = point.Item1;
                var pointValue = point.Item2;
                norm += Math.Abs(pointValue - function.Value(pointVector));
            }
            return norm;
        }
    }
}
