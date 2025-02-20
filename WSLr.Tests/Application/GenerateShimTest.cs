namespace WSLr.Tests.Application;

[TestClass]
public class GenerateShimTest
{
    [TestMethod]
    public async Task WithBinary_DoesNotError()
    {
        // Arrange
        var subject = CreateSubject();
        var shimBinary = ShimBinary.From("ls");
        var runtime = TestRuntime.New();

        // Act
        var result = await subject.With(shimBinary).Run(runtime);

        // Assert
        result.Should().BeSuccess();
    }

    [TestMethod]
    public async Task WithBinary_WritesOutput()
    {
        // Arrange
        var mocks = new Mocks();
        var subject = CreateSubject(mocks);
        var shimBinary = ShimBinary.From("ls");
        var runtime = TestRuntime.New();

        // Act
        await subject.With(shimBinary).Run(runtime);

        // Assert
        mocks.OutputWriter.Verify(x => x.Write(It.IsAny<OutputData>()));
    }

    private GenerateShim<TestRuntime> CreateSubject(Mocks? mocks = default)
    {
        mocks ??= new();
        return new(mocks.OutputWriter.Object);
    }
}