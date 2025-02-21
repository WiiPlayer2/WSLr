using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;

namespace WSLr.Implementations.DotnetPublishShimBuilder;

public static class DI
{
    public static void AddDotnetPublishShimBuilder<RT>(this IServiceCollection services) where RT : struct, HasCancel<RT>
    {
        services.AddTransient<IShimBuilder<RT>, DotnetPublishShimBuilder<RT>>();
    }
}
