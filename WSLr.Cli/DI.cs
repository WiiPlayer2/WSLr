using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;
using WSLr.Cli.NullServices;

namespace WSLr.Cli;

public static class DI
{
    public static void AddNullServices<RT>(this IServiceCollection services) where RT : struct, HasCancel<RT>
    {
        services.AddTransient<IOutputWriter<RT>, NullOutputWriter<RT>>();
        services.AddTransient<IShimBuilder<RT>, NullShimBuilder<RT>>();
    }
}
