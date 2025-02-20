using LanguageExt.Effects.Traits;

namespace WSLr.Application;

public class GenerateShim<RT>(IOutputWriter<RT> outputWriter, IShimBuilder<RT> shimBuilder)
    where RT : struct, HasCancel<RT>
{
    public Aff<RT, Unit> With(ShimTarget shimTarget)
    {
        shimBuilder.Build(new());
        return outputWriter.Write(OutputData.From(Array<byte>()));
    }
}
