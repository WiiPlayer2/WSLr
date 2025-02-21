using LanguageExt.Effects.Traits;

namespace WSLr.Implementations.FileOutputWriter;

internal class FileOutputWriter<RT> : IOutputWriter<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, Unit> Write(OutputPath path, OutputData data) =>
        Aff((RT rt) => File.WriteAllBytesAsync(path.Value, data.Value.ToArray()).ToUnit().ToValue());
}
