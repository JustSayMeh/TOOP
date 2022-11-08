using Functions;
using Geometry;
using System;
using System.Collections.Generic;

namespace Functionals
{
    class L2Norm : IDifferentiableFunctional, ILeastSquaresFunctional
    {
        private List<(IVector, double)> points;
        public L2Norm(List<(IVector, double)> points)
        {
            this.points = points;
        }
        public IVector Gradient(IDifferentiableFunction function)
        {
            double[] result = null;
            double norm = 0;
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var pointVector = point.Item1;
                var pointValue = point.Item2;
                double value = pointValue - function.Value(pointVector);
                norm += value * value;
                var gradient = function.Gradient(pointVector);
                if (result == null)
                    result = new double[gradient.Size];
                for (int j = 0; j < gradient.Size; j++)
                {
                    result[j] += -2 * value * gradient[j];
                }
            }
            return new VectorImpl(result).Multiple(1 / (2 * Math.Sqrt(norm)));
        }

        public double Value(IFunction function)
        {
            double norm = 0;
            foreach (var point in points)
            {
                var pointVector = point.Item1;
                var pointValue = point.Item2;
                double value = pointValue - function.Value(pointVector);
                norm += value * value;
            }
            return Math.Sqrt(norm);
        }

        public IVector Residual(IFunction function)
        {
            double[] vector = new double[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var pointVector = point.Item1;
                var pointValue = point.Item2;
                double value = function.Value(pointVector) - pointValue;
                vector[i] = value;
            }
            return VectorImpl.Of(vector);
        }

        public IMatrix Jacobian(IDifferentiableFunction function)
        {
            double[,] arr = null;
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var pointVector = point.Item1;
                var grad = function.Gradient(pointVector);
                if (arr == null)
                    arr = new double[points.Count, grad.Size];
                for (int j = 0; j < grad.Size; j++ ) {
                    arr[i, j] = grad[j];
                }
            }
            return new MatrixImpl(arr);
        }
    }
}
