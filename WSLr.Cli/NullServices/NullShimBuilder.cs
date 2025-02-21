using LanguageExt.Effects.Traits;

namespace WSLr.Cli.NullServices;

internal class NullShimBuilder<RT> : IShimBuilder<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, OutputData> Build(ShimBuildConfig buildConfig) => SuccessAff(OutputData.From(Array<byte>()));
}
