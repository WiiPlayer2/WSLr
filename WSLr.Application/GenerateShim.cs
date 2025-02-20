using LanguageExt.Effects.Traits;

namespace WSLr.Application;

public class GenerateShim<RT>(IOutputWriter<RT> outputWriter)
    where RT : struct, HasCancel<RT>
{
    public Aff<RT, Unit> With(ShimBinary shimBinary) => outputWriter.Write(OutputData.From(Array<byte>()));
}
