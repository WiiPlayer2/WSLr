using LanguageExt.Effects.Traits;

namespace WSLr.Application;

public interface IOutputWriter<RT> where RT : struct, HasCancel<RT>
{
    Aff<RT, Unit> Write(OutputData data);
}
