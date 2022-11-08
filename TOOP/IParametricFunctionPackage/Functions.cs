using Geometry;

namespace Functions

{

    public interface IParametricFunction
    {
        IFunction Bind(IVector parameters);
    }



    public interface IFunction
    {
        double Value(IVector point);
    }


    public interface IDifferentiableFunction : IFunction
    {
        // По параметрам исходной IParametricFunction
        IVector Gradient(IVector point);
    }

}
