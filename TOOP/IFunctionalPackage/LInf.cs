using Functions;
using Geometry;
using System;
using System.Collections.Generic;

namespace Functionals { 
    class LInf : IFunctional
    {
        private List<(IVector, double)> points;

        public LInf(List<(IVector, double)> points)
        {
            this.points = points;
        }

        public double Value(IFunction function)
        {
            double norm = 0;
            foreach (var point in points)
            {
                var pointVector = point.Item1;
                var pointValue = point.Item2;
                var Lnorm = Math.Abs(pointValue - function.Value(pointVector));
                if (Lnorm > norm)
                    norm = Lnorm;
            }
            return norm;
        }
    }
}
