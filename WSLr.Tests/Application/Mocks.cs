using System;
using Moq;

namespace WSLr.Tests.Application;

internal class Mocks
{
    public Mocks()
    {
        OutputWriter.Setup(x => x.Write(It.IsAny<OutputData>()))
            .Returns(unitAff);
        ShimBuilder.Setup(x => x.Build(It.IsAny<ShimBuildConfig>()))
            .Returns(SuccessAff(OutputData.From(Array<byte>())));
    }
    
    public Mock<IOutputWriter<TestRuntime>> OutputWriter { get; } = new();
    
    public Mock<IShimBuilder<TestRuntime>> ShimBuilder { get; } = new();
}
