

using System;
using System.Text;

namespace Geometry
{
    public interface IVector {
        public int Size { get; }
        public double[] Elements { get; }
        IVector Add(IVector vector);
        IVector Subtract(IVector vector);
        IVector Multiple(IVector vector);
        IVector Multiple(double a);
        IVector Divide(double a);
        IVector Invers();
        IVector Copy();
        double Scalar(IVector vector);
        double Norm();
        public double this[int i]
        {
            get => Elements[i];
            set => Elements[i] = value;
        }
    }
    public interface IMatrix {
        public (int, int) Size { get; }
        public double[,] Elements { get; }
        IMatrix Add(IMatrix a);
        IMatrix Subtract(IMatrix a);
        IMatrix Multiple(IMatrix a);
        IVector Multiple(IVector a);
        IMatrix Transpose();
        IMatrix Inverse();
        public double this[int i, int j]
        {
            get => Elements[i, j];
            set => Elements[i, j] = value;
        }
    }

    public class VectorImpl : IVector
    {
        delegate double ElementTransformation(int index, double element);
        private double[] elements;
        public int Size => elements.Length;
        public double[] Elements => elements;

        private void checkDimensions(IVector vector)
        {
            if (vector.Size != Size)
            {
                throw new ArgumentException("Vectors must have the same dimensions");
            }
        }
        private double[] iterate(ElementTransformation elementTransformation)
        {
            double[] arr = new double[Size];
            for (int i = 0; i < Size; i++)
            {
                arr[i] = elementTransformation(i, elements[i]);
            }
            return arr;
        }
        public VectorImpl(int n)
        {
            elements = new double[n];
        }

        public VectorImpl(double[] arr)
        {
            elements = new double[arr.Length];
            Array.Copy(arr, 0, elements, 0, arr.Length);
        }

        public IVector Add(IVector vector)
        {
            checkDimensions(vector);
            double[] arr = iterate((i, element) => vector.Elements[i] + element);
            return new VectorImpl(arr);
        }

        public IVector Subtract(IVector vector)
        {
            checkDimensions(vector);
            double[] arr = iterate((i, element) => element - vector.Elements[i]);
            return new VectorImpl(arr);
        }

        public IVector Copy()
        {
            return new VectorImpl(elements);
        }

        public IVector Divide(double a)
        {
            double[] arr = iterate((i, element) => element / a);
            return new VectorImpl(arr);
        }

        public IVector Multiple(IVector vector)
        {
            checkDimensions(vector);
            double[] arr = iterate((i, element) => element * vector.Elements[i]);
            return new VectorImpl(arr);
        }

        public IVector Multiple(double a)
        {
            double[] arr = iterate((i, element) => element * a);
            return new VectorImpl(arr);
        }

        public double Norm()
        {
            double acc = 0;
            for (int i = 0; i < Size; i++)
            {
                double element = elements[i];
                acc += element * element;
            }
            return Math.Sqrt(acc);
        }

        public IVector Invers()
        {
            double[] arr = iterate((i, element) => -element);
            return new VectorImpl(arr);
        }

        public double Scalar(IVector vector)
        {
            checkDimensions(vector);
            double acc = 0;
            for (int i = 0; i < Size; i++)
            {
                acc += vector.Elements[i] * elements[i];
            }
            return acc;
        }

        public static IVector Of(params double[] arr)
        {
            return new VectorImpl(arr);
        }

        public override string ToString()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("(");
            for (int i = 0; i < Size - 1; i++)
            {
                strb.Append($"{ elements[i]}, ");
            
            }
            strb.Append($"{elements[Size - 1]}");
            strb.Append(")");
            return strb.ToString();
        }
    }

    public class MatrixImpl : IMatrix
    {
        delegate double ElementTransformation(int i, int j, double element);
        private double[,] elements;
        public (int, int) Size => (elements.GetLength(0), elements.GetLength(1));

        public double[,] Elements => elements;
        private double[,] iterate(ElementTransformation elementTransformation)
        {
            double[,] arr = new double[Size.Item1, Size.Item2];
            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    arr[i, j] = elementTransformation(i, j, elements[i, j]);
                }
            }
            return arr;
        }
        private void checkDimensions(IMatrix matrix)
        {
            if (matrix.Size != Size)
            {
                throw new ArgumentException("Vectors must have the same dimensions");
            }
        }
        public MatrixImpl(int n, int m)
        {
            elements = new double[n, m];
        }

        public MatrixImpl(double[,] arr)
        {
            if (arr.Rank != 2)
            {
                throw new ArgumentException("Invalid array dimension!");
            }
            elements = new double[arr.GetLength(0), arr.GetLength(1)];
            Array.Copy(arr, 0, elements, 0, arr.Length);
        }

        public IMatrix Add(IMatrix a)
        {
            checkDimensions(a);
            var arr = iterate((i, j, item) => item + a.Elements[i, j]);
            return new MatrixImpl(arr);
        }

        public IMatrix Inverse()
        {
            double[,] a = new double[Size.Item1, 2 * Size.Item2];
            for (int i = 0; i < Size.Item1; i++)
            {
                for (int j = 0; j < Size.Item2; j++)
                {
                    a[i, j] = elements[i, j];
                }
                a[i, Size.Item2 + i] = 1;
            }
            int index = 0;
            while (index < Size.Item2)
            {
                if (a[index, index] == 0)
                {
                    for (int k = index; k < a.GetLength(0); k++)
                    {
                        if (a[k, index] != 0)
                        {
                            swapRow(index, k, a);
                            break;
                        }
                    }
                }
                multRow(index, 1 / a[index, index], a);
                for (int i = index + 1; i < a.GetLength(0); i++)
                {
                    if (a[i, index] != 0)
                        multRow(i, 1.0 / a[i, index], a);
                    subtractRow(i, index, 1, a);
                }
                index += 1;
            }
            index -= 1;

            while(index > 0)
            {
                for (int k = index - 1; k >= 0; k--)
                {
                    subtractRow(k, index, a[k, index], a);
                }
                index -= 1;
            }
            double[,] inverse = new double[Size.Item1, Size.Item2];
            for (int i = 0; i < Size.Item1; i++)
            {
                for (int j = 0; j < Size.Item2; j++)
                {
                    inverse[i, j] = a[i, j + Size.Item2];
                }
            }
            return new MatrixImpl(inverse);
        }

        private void swapRow(int i, int j, double[,] arr)
        {
            for (int k = 0; k < arr.GetLength(1); k++)
            {
                double acc = arr[i, k];
                arr[i, k] = arr[j, k];
                arr[j, k] = acc;
            }
        }

        private void multRow(int i, double mult, double[,] arr)
        {
            for (int k = 0; k < arr.GetLength(1); k++)
            {
                arr[i, k] *= mult;
            }
        }

        private void subtractRow(int i, int j, double coef, double[,] arr)
        {
            for (int k = 0; k < arr.GetLength(1); k++)
            {
                arr[i, k] = arr[i, k] - arr[j, k] * coef;
            }
        }

        public IMatrix Multiple(IMatrix a)
        {
            if (Size.Item2 != a.Size.Item1)
            {
                throw new ArgumentException();
            }
            double[,] arr = new double[Size.Item1, a.Size.Item2];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    double acc = 0;
                    for (int k = 0; k < Size.Item2; k++)
                    {
                        acc += elements[i, k] * a.Elements[k, j];
                    }
                    arr[i, j] = acc;
                }
                
            }
            return new MatrixImpl(arr);
        }

        public IVector Multiple(IVector a)
        {
            double[] arr = new double[Size.Item1];
            for (int i = 0; i < arr.Length; i++)
            {
                double acc = 0;
                for (int j = 0; j < Size.Item2; j++)
                {
                    acc += a[j] * elements[i, j];
                }
                arr[i] = acc;
            }
            return new VectorImpl(arr);
        }

        public IMatrix Subtract(IMatrix a)
        {
            checkDimensions(a);
            var arr = iterate((i, j, item) => item - a.Elements[i, j]);
            return new MatrixImpl(arr);
        }

        public IMatrix Transpose()
        {
            double[,] arr = new double[Size.Item2, Size.Item1];
            for (int i = 0; i < elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    arr[j, i] = elements[i, j];
                }
            }
            return new MatrixImpl(arr);
        }
    }
}
