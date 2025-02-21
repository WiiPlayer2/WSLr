using LanguageExt.Effects.Traits;

namespace WSLr.Cli.NullServices;

internal class NullOutputWriter<RT> : IOutputWriter<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, Unit> Write(OutputData data) => unitAff;
}
