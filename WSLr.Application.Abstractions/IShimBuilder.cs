using LanguageExt.Effects.Traits;

namespace WSLr.Application;

public interface IShimBuilder<RT> where RT : struct, HasCancel<RT>
{
    Aff<RT, OutputData> Build(ShimBuildConfig buildConfig);
}