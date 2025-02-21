using System.CommandLine;

namespace WSLr.Cli.Commands;

internal static class Arguments
{
    public static Argument<string> Target { get; } = new(
        "target"
    );
}
