using Functions;
using Geometry;

namespace Functionals

{
    public interface IFunctional
    {
        double Value(IFunction function);
    }

    public interface IDifferentiableFunctional : IFunctional
    {
        IVector Gradient(IDifferentiableFunction function);
    }

    public interface ILeastSquaresFunctional : IFunctional
    {
        IVector Residual(IFunction function);
        IMatrix Jacobian(IDifferentiableFunction function);
    }

}
