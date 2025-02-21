namespace WSLr.Implementations.DotnetPublishShimBuilder;

internal record ProjectFilesDirectory(DirectoryInfo Path) : IDisposable
{
    public void Dispose() => Path.Delete(true);
}
