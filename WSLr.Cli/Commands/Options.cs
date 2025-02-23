namespace WSLr.Cli.Commands;

public static class Options
{
    public static System.CommandLine.Option<bool> FixInputLineEndings { get; } = new(
        ["--fix-input-line-endings"],
        () => false
    );
}
