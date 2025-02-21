using System.CommandLine;

namespace WSLr.Cli.Commands;

public class GenerateShimCommand : Command
{
    public GenerateShimCommand() : base("generate-shim")
    {
        Add(Commands.Arguments.Target);
    }
}
