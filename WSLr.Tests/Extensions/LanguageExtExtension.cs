using FluentAssertions.Execution;

namespace WSLr.Tests.Extensions;

internal static class LanguageExtExtension
{
    public static FinAssertions<T> Should<T>(this Fin<T> value) => new(value, AssertionChain.GetOrCreate());
}