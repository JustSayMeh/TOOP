using Geometry;
using System;
using System.Collections.Generic;
using static Functions.Linear;

namespace Functions
{
    class PiecewiseLinear : IParametricFunction
    {
        private List<(IVector, IVector)> intervals;
        public PiecewiseLinear(List<(IVector, IVector)> intervals)
        {
            this.intervals = intervals;
        }
        public IFunction Bind(IVector parameters)
        {
            int paramsSize = parameters.Size / intervals.Count;
            List<IVector> paramsList = new List<IVector>(); 
            for (int i = 0; i < intervals.Count; i+= paramsSize)
            {
                paramsList.Add(VectorImpl.Of(parameters.Elements[i..(i + paramsSize)]));
            }
           
            return new PiecewiseLinearFunction(intervals, paramsList);
        }
    }

    class PiecewiseLinearFunction : IDifferentiableFunction
    {
        private IntervalTree tree;
        private int paramsSize;
        public PiecewiseLinearFunction(List<(IVector, IVector)> intervals, List<IVector> parameters) 
        {
            var list = new List<(IVector, IVector, IFunction)>();
            var function = new LinearParametricFunction();
            for (int i = 0; i < intervals.Count; i++)
            {
                var item = intervals[i];
                list.Add((item.Item1, item.Item2, function.Bind(parameters[i])));
            }
            paramsSize = parameters[0].Size;
            tree = new IntervalTree(list);
        }

        public IVector Gradient(IVector point)
        {
            var function = (IDifferentiableFunction) tree.GetFunction(point);
            if (function == null)
            {
                return VectorImpl.Of(new double[paramsSize]);
            }
            return function.Gradient(point);
        }

        public double Value(IVector point)
        {
            var function = tree.GetFunction(point);
            if (function == null)
            {
                return 0;
            }
            return function.Value(point);
        }
    }

    class IntervalTree
    {
        public List<(IVector, IVector, IFunction)> intervals { get; private set; }
        public IntervalTree(List<(IVector, IVector, IFunction)> intervals)
        {
            this.intervals = new List<(IVector, IVector, IFunction)>();
            foreach (var interval in intervals)
            {
                var intervalStart = interval.Item1;
                var intervalEnd = interval.Item2;
                for (int j = 0; j < interval.Item1.Size; j++)
                if (intervalStart[j] >= intervalEnd[j])
                    throw new ArgumentException("Interval start must be less then interval end");
                this.intervals.Add(interval);
            }
        }

        public IFunction GetFunction(IVector point)
        {
            foreach (var interval in intervals)
            {
                bool flag = true;
                for (int i = 0; i < point.Size; i++)
                {
                    double coord = point[i];
                    var coords = interval;
                    if (interval.Item1[i] > coord || coords.Item2[i] < coord)
                        flag = false;
                }
                if (flag)
                    return interval.Item3;
            }
            return null;
        }
    }
}
