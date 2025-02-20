namespace WSLr.Tests.Application;

[TestClass]
public class GenerateShimTest
{
    [TestMethod]
    public async Task WithTarget_DoesNotError()
    {
        // Arrange
        var subject = CreateSubject();
        var shimTarget = ShimTarget.From("ls");
        var runtime = TestRuntime.New();

        // Act
        var result = await subject.With(shimTarget).Run(runtime);

        // Assert
        result.Should().BeSuccess();
    }

    [TestMethod]
    public async Task WithTarget_WritesOutput()
    {
        // Arrange
        var mocks = new Mocks();
        var subject = CreateSubject(mocks);
        var shimTarget = ShimTarget.From("ls");
        var runtime = TestRuntime.New();

        // Act
        await subject.With(shimTarget).Run(runtime);

        // Assert
        mocks.OutputWriter.Verify(x => x.Write(It.IsAny<OutputData>()));
    }

    [TestMethod]
    public async Task WithTarget_BuildsShim()
    {
        // Arrange
        var mocks = new Mocks();
        var subject = CreateSubject(mocks);
        var shimTarget = ShimTarget.From("ls");
        var runtime = TestRuntime.New();

        // Act
        await subject.With(shimTarget).Run(runtime);
        
        // Assert
        mocks.ShimBuilder.Verify(x => x.Build(It.IsAny<ShimBuildConfig>()));
    }

    [TestMethod]
    public async Task WithTarget_BuildsShimUsingTargetAndWritesOutputSuccessfully()
    {
        // Arrange
        var shimTarget = ShimTarget.From("ls");
        var expectedBuildConfig = new ShimBuildConfig(shimTarget);
        var expectedOutputData = OutputData.From(Array<byte>(0xCA, 0xFE));
        var mocks = new Mocks();
        mocks.ShimBuilder.Setup(x => x.Build(It.Is<ShimBuildConfig>(x => x == expectedBuildConfig)))
            .Returns(SuccessAff(expectedOutputData));
        var subject = CreateSubject(mocks);
        var runtime = TestRuntime.New();

        // Act
        await subject.With(shimTarget).Run(runtime);
        
        // Assert
        mocks.OutputWriter.Verify(x => x.Write(It.Is<OutputData>(x => x == expectedOutputData)));
    }

    private GenerateShim<TestRuntime> CreateSubject(Mocks? mocks = default)
    {
        mocks ??= new();
        return new(
            mocks.OutputWriter.Object,
            mocks.ShimBuilder.Object);
    }
}