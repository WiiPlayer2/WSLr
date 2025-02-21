namespace WSLr.Implementations.RoslynShimBuilder;

internal record ProjectFilesDirectory(DirectoryInfo Path) : IDisposable
{
    public void Dispose() => Path.Delete(true);
}
