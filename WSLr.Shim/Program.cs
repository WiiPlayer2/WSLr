using CliWrap;
using WSLr.Shim;

var execConfig = LoadExecConfig();
DebugWriteLine("<init>");
DebugExecConfig();

DebugWriteLine("<exec>");
var result = await Cli.Wrap("wsl")
    .WithArguments(builder => builder
        .Add(execConfig.Binary)
        .Add(args))
    .WithStandardInputPipe(PipeSource.FromStream(
        execConfig.FixInputLineEndings
            ? new LineEndingStream(Console.OpenStandardInput())
            : Console.OpenStandardInput()))
    .WithStandardOutputPipe(PipeTarget.ToStream(Console.OpenStandardOutput()))
    .WithStandardErrorPipe(PipeTarget.ToStream(Console.OpenStandardError()))
    .WithValidation(CommandResultValidation.None)
    .ExecuteAsync();

DebugWriteLine("<exit>");
DebugResult();

return result.ExitCode;

void DebugWriteLine(string format, params object[] args)
{
    if (!execConfig.DebugEnabled)
        return;

    Console.Error.WriteLine($"[WSLr] {string.Format(format, args)}");
}

void DebugExecConfig()
{
    DebugWriteLine("Binary: {0}", execConfig.Binary);
    DebugWriteLine("Arguments: {0}", string.Join(" ", args));
}

void DebugResult()
{
    DebugWriteLine("Exit Code: {0}", result.ExitCode);
}

ExecConfig LoadExecConfig()
{
    const string envPrefix = "WSLR_SHIM_";
    const string envDebugEnabled = $"{envPrefix}DEBUG_ENABLED";
    const string envBinary = $"{envPrefix}BINARY";
    const string envFixInputLineEndings = $"{envPrefix}FIX_INPUT_LINE_ENDINGS";

    return new ExecConfig(
        LoadBool(envDebugEnabled, ThisAssembly.Constants.ShimDefaults.DebugEnabled),
        LoadString(envBinary, ThisAssembly.Constants.ShimDefaults.Binary),
        LoadBool(envFixInputLineEndings, ThisAssembly.Constants.ShimDefaults.FixInputLineEndings));

    bool LoadBool(string environmentVariable, bool defaultValue) =>
        LoadEnv(environmentVariable, x => string.Equals(x, "true", StringComparison.InvariantCultureIgnoreCase), defaultValue);

    string LoadString(string environmentVariable, string defaultValue) =>
        LoadEnv(environmentVariable, x => x, defaultValue);

    T LoadEnv<T>(string environmentVariable, Func<string, T> map, T defaultValue)
    {
        var envValue = Environment.GetEnvironmentVariable(environmentVariable);
        return envValue is null
            ? defaultValue
            : map(envValue);
    }
}

internal record ExecConfig(
    bool DebugEnabled,
    string Binary,
    bool FixInputLineEndings);