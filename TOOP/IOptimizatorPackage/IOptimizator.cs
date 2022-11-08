using Functionals;
using Functions;
using Geometry;

namespace Optimizators
{
    interface IOptimizator
    {

        IVector Minimize(IFunctional objective,

                         IParametricFunction function,

                         IVector initialParameters,

                         IVector minimumParameters = default,

                         IVector maximumParameters = default);

    }
}
