using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;

namespace WSLr.Application;

public static class DI
{
    public static void AddApplicationServices<RT>(this IServiceCollection services) where RT : struct, HasCancel<RT>
    {
        services.AddTransient<GenerateShim<RT>>();
    }
}
