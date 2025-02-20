using System;
using LanguageExt.Effects.Traits;

namespace WSLr.Tests.Application;

public struct TestRuntime : HasCancel<TestRuntime>
{
    private TestRuntime(CancellationTokenSource cancellationTokenSource)
    {
        CancellationTokenSource = cancellationTokenSource;
        CancellationToken = cancellationTokenSource.Token;
    }

    public static TestRuntime New() => new(new CancellationTokenSource());

    public TestRuntime LocalCancel => this;

    public CancellationToken CancellationToken { get; }

    public CancellationTokenSource CancellationTokenSource { get; }
}
