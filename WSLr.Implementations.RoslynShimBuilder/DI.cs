using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;
using WSLr.Application;

namespace WSLr.Implementations.RoslynShimBuilder;

public static class DI
{
    public static void AddRoslynShimBuilder<RT>(this IServiceCollection services) where RT : struct, HasCancel<RT>
    {
        services.AddTransient<IShimBuilder<RT>, RoslynShimBuilder<RT>>();
    }
}
