namespace WSLr.Domain;

public static class GetOutputFilename
{
    public static OutputPath With(ShimTarget target) => OutputPath.From("ls.exe");
}
