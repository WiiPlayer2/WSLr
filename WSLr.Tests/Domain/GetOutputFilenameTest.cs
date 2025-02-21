namespace WSLr.Tests.Domain;

[TestClass]
public class GetOutputFilenameTest
{
    [DataRow("ls", "ls.exe")]
    [TestMethod]
    public void WithValidTarget_ReturnsValidOutputPath(string target, string expectedPath)
    {
        // Arrange
        var shimTarget = ShimTarget.From(target);
        var expectedOutputPath = OutputPath.From(expectedPath);

        // Act
        var result = GetOutputFilename.With(shimTarget);

        // Assert
        result.Should().Be(expectedOutputPath);
    }
}
