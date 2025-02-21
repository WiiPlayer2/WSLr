using System.CommandLine.Invocation;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.Logging;

namespace WSLr.Cli.Commands.Handlers;

internal static class CommandHandlingExtension
{
    public static Task<int> RunCommand<RT>(this Aff<RT, Unit> aff, CommandRuntime<RT> commandRuntime, ILogger logger)
        where RT : struct, HasCancel<RT> =>
        aff.Run(commandRuntime.Runtime)
            .Map(fin => fin.Match(
                _ => 0,
                error =>
                {
                    logger.LogError(error.ToException(), "Command failed");
                    return -1;
                }))
            .AsTask();
}

public class GenerateShimCommandHandler<RT>(CommandRuntime<RT> commandRuntime, GenerateShim<RT> generateShim, ILogger<GenerateShimCommand> logger) : ICommandHandler
    where RT : struct, HasCancel<RT>
{
    public int Invoke(InvocationContext context) => throw new NotImplementedException();

    public Task<int> InvokeAsync(InvocationContext context) =>
    (
        from shimTarget in Eff(() => context.ParseResult.GetValueForArgument(Arguments.Target))
            .Map(ShimTarget.From)
        from _ in generateShim.With(shimTarget)
        select unit
    ).RunCommand(commandRuntime, logger);
}
