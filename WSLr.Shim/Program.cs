using CliWrap;

var debugEnabled = ThisAssembly.Constants.ShimDefaults.DebugEnabled;

DebugWriteLine("<init>");
var wslBinary = ThisAssembly.Constants.ShimDefaults.Binary;
DebugWriteLine("Binary: {0}", wslBinary);
DebugWriteLine("Arguments: {0}", string.Join(" ", args));


DebugWriteLine("<exec>");
var result = await Cli.Wrap("wsl")
    .WithArguments(builder => builder
        .Add(wslBinary)
        .Add(args))
    .WithStandardInputPipe(PipeSource.FromStream(Console.OpenStandardInput()))
    .WithStandardOutputPipe(PipeTarget.ToStream(Console.OpenStandardOutput()))
    .WithStandardErrorPipe(PipeTarget.ToStream(Console.OpenStandardError()))
    .WithValidation(CommandResultValidation.None)
    .ExecuteAsync();

DebugWriteLine("<exit>");
DebugWriteLine("Exit Code: {0}", result.ExitCode);

return result.ExitCode;

void DebugWriteLine(string format, params object[] args)
{
    if (!debugEnabled)
        return;

    Console.Error.WriteLine($"[WSLr] {string.Format(format, args)}");
}