using LanguageExt.Effects.Traits;
using WSLr.Implementation.ShimProject;

namespace WSLr.Implementations.RoslynShimBuilder;

internal class RoslynShimBuilder<RT> : IShimBuilder<RT> where RT : struct, HasCancel<RT>
{
    public Aff<RT, OutputData> Build(ShimBuildConfig buildConfig) =>
        use<RT, ProjectFilesDirectory, OutputData>(
            LoadProjectFiles(),
            directory =>
                SuccessAff(OutputData.From(Array<byte>())));

    private Eff<RT, ProjectFilesDirectory> GetTemporaryProjectFilesDirectory() =>
        from tempDirectory in Eff(() => Directory.CreateTempSubdirectory("WSLr.Shim"))
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
