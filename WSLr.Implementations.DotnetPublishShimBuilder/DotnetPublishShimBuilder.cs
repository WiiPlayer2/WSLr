using CliWrap;
using LanguageExt.Effects.Traits;
using WSLr.Implementation.ShimProject;

namespace WSLr.Implementations.DotnetPublishShimBuilder;

internal class DotnetPublishShimBuilder<RT> : IShimBuilder<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, OutputData> Build(ShimBuildConfig buildConfig) =>
        use(
            LoadProjectFiles(),
            projectFilesDirectory =>
                from projectFilePath in Eff(() => Path.Join(projectFilesDirectory.Path.FullName, ProjectFiles.ProjectFileName))
                from outputPath in Eff(() => Path.Join(projectFilesDirectory.Path.FullName, "out"))
                from publishCliResult in Aff(async (RT rt) => await Cli.Wrap("dotnet")
                    .WithArguments(builder => builder
                        .Add([
                            "publish",
                            projectFilePath,
                            "--configuration", "Release",
                            "--runtime", "win-x64",
                            "--self-contained",
                            "-p:PublishSingleFile=true",
                            "-p:PublishTrimmed=true",
                            "--output", outputPath,
                            $"-p:ShimDefaultBinary={buildConfig.shimTarget.Value}",
                        ]))
                    .ExecuteAsync(rt.CancellationToken))
                from shimOutputPath in Eff(() => Path.Join(projectFilesDirectory.Path.FullName, "out", ProjectFiles.ShimOutputFileName))
                from shimData in Aff((RT rt) => File.ReadAllBytesAsync(shimOutputPath).ToValue())
                select OutputData.From(toArray(shimData))
        );

    private Eff<RT, ProjectFilesDirectory> GetTemporaryProjectFilesDirectory() =>
        from tempDirectory in Eff(() => Directory.CreateTempSubdirectory("WSLr.Shim_"))
        select new ProjectFilesDirectory(tempDirectory);

    private Aff<RT, ProjectFilesDirectory> LoadProjectFiles() =>
        from directory in GetTemporaryProjectFilesDirectory()
        from filesMap in Eff(() => toSeq(ProjectFiles.GetProjectFiles()))
        from _ in (
                from file in filesMap
                select use(
                    SuccessEff(file.Stream),
                    stream =>
                        use(
                            Eff(() => File.OpenWrite(Path.Join(directory.Path.FullName, file.Filename))),
                            targetStream =>
                                Aff(() => stream.CopyToAsync(targetStream).ToUnit().ToValue())))
            )
            .TraverseParallel(identity)
        select directory;
}
