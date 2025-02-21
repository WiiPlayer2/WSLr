namespace WSLr.Implementation.ShimProject;

public static class ProjectFiles
{
    public static string ProjectFileName => "WSLr.Shim.csproj";

    public static string ShimOutputFileName => "WSLr.Shim.exe";

    public static IEnumerable<ProjectFile> GetProjectFiles() =>
        typeof(ProjectFiles).Assembly.GetManifestResourceNames()
            .Select(x =>
            {
                var filename = x[(typeof(ProjectFiles).Namespace!.Length + ".ProjectFiles.".Length)..];
                var stream = typeof(ProjectFiles).Assembly.GetManifestResourceStream(x)!;
                return new ProjectFile(
                    filename,
                    stream);
            });
}

public record ProjectFile(string Filename, Stream Stream) : IDisposable
{
    public void Dispose() => Stream.Dispose();
}
