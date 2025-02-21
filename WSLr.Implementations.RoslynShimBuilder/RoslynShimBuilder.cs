using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Effects.Traits;
using WSLr.Application;
using WSLr.Domain;

namespace WSLr.Implementations.RoslynShimBuilder;

internal class RoslynShimBuilder<RT> : IShimBuilder<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, OutputData> Build(ShimBuildConfig buildConfig) => throw new NotImplementedException();
}
