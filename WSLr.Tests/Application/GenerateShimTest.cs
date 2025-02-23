namespace WSLr.Tests.Application;

[TestClass]
public class GenerateShimTest
{
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
        var shimFixStreamLineEndings = ShimFixInputLineEndings.From(false);
        var expectedBuildConfig = new ShimBuildConfig(shimTarget, shimFixStreamLineEndings);
        var expectedOutputFile = OutputPath.From("ls.exe");
        var expectedOutputData = OutputData.From(Array<byte>(0xCA, 0xFE));
        var mocks = new Mocks();
        mocks.ShimBuilder.Setup(x => x.Build(expectedBuildConfig))
            .Returns(SuccessAff(expectedOutputData));
        var subject = CreateSubject(mocks);
        var runtime = TestRuntime.New();

        // Act
        await subject.With(shimTarget).Run(runtime);

        // Assert
        mocks.OutputWriter.Verify(x => x.Write(expectedOutputFile, expectedOutputData));
    }

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
        mocks.OutputWriter.Verify(x => x.Write(It.IsAny<OutputPath>(), It.IsAny<OutputData>()));
    }

    [TestMethod]
    public async Task WithTargetAndFixInputLineEndings_BuildsShim()
    {
        // Arrange
        var mocks = new Mocks();
        var subject = CreateSubject(mocks);
        var shimTarget = ShimTarget.From("ls");
        var fixInputLineEndings = ShimFixInputLineEndings.From(true);
        var runtime = TestRuntime.New();
        var expectedShimBuildConfig = new ShimBuildConfig(shimTarget, fixInputLineEndings);

        // Act
        await subject.With(shimTarget, fixInputLineEndings).Run(runtime);

        // Assert
        mocks.ShimBuilder.Verify(x => x.Build(expectedShimBuildConfig));
    }

    private GenerateShim<TestRuntime> CreateSubject(Mocks? mocks = default)
    {
        mocks ??= new();
        return new(
            mocks.OutputWriter.Object,
            mocks.ShimBuilder.Object);
    }
}
