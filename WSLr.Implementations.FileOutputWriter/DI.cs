using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;

namespace WSLr.Implementations.FileOutputWriter;

public static class DI
{
    public static void AddFileOutputWriter<RT>(this IServiceCollection services) where RT : struct, HasCancel<RT>
    {
        services.AddTransient<IOutputWriter<RT>, FileOutputWriter<RT>>();
    }
}
