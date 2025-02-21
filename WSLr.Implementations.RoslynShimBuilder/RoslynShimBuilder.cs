using LanguageExt.Common;
using LanguageExt.Effects.Traits;
using Microsoft.CodeAnalysis.MSBuild;
using WSLr.Implementation.ShimProject;

namespace WSLr.Implementations.RoslynShimBuilder;

internal class RoslynShimBuilder<RT> : IShimBuilder<RT> where RT : struct, HasCancel<RT>
{
    // private static readonly Type __csharp = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

    public Aff<RT, OutputData> Build(ShimBuildConfig buildConfig) =>
        use<RT, ProjectFilesDirectory, OutputData>(
            LoadProjectFiles(),
            directory =>
                use<RT, MSBuildWorkspace, OutputData>(
                    Eff(MSBuildWorkspace.Create),
                    workspace =>
                        from projectFilePath in Eff(() => Path.Join(directory.Path.FullName, ProjectFiles.ProjectFileName))
                        from project in Aff((RT rt) => workspace.OpenProjectAsync(projectFilePath, cancellationToken: rt.CancellationToken).ToValue())
                        from compilation in Aff((RT rt) => project.GetCompilationAsync(rt.CancellationToken).ToValue())
                        from outputData in use(
                            Eff(() => new MemoryStream()),
                            outputStream =>
                                from emitResult in Eff((RT rt) => compilation.Emit(outputStream, cancellationToken: rt.CancellationToken))
                                from _ in guard(emitResult.Success, Error.New("Compilation failed."))
                                select OutputData.From(toArray(outputStream.ToArray())))
                        select outputData
                )
        );

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
