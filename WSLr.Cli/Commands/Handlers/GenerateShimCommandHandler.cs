using System.CommandLine.Invocation;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.Logging;

namespace WSLr.Cli.Commands.Handlers;

public class GenerateShimCommandHandler<RT>(CommandRuntime<RT> commandRuntime, GenerateShim<RT> generateShim, ILogger<GenerateShimCommand> logger) : ICommandHandler
    where RT : struct, HasCancel<RT>
{
    public int Invoke(InvocationContext context) => throw new NotImplementedException();

    public Task<int> InvokeAsync(InvocationContext context) =>
    (
        from shimTarget in Eff(() => context.ParseResult.GetValueForArgument(Arguments.Target))
            .Map(ShimTarget.From)
        from shimFixInputLineEndings in Eff(() => context.ParseResult.GetValueForOption(Options.FixInputLineEndings))
            .Map(ShimFixInputLineEndings.From)
        from _ in generateShim.With(shimTarget, shimFixInputLineEndings)
        select unit
    ).RunCommand(commandRuntime, logger);
}
