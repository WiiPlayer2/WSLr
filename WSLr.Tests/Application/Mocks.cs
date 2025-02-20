using System;
using Moq;

namespace WSLr.Tests.Application;

internal class Mocks
{
    public Mocks()
    {
        OutputWriter.Setup(x => x.Write(It.IsAny<OutputData>())).Returns(unitAff);
    }
    
    public Mock<IOutputWriter<TestRuntime>> OutputWriter { get; } = new();
}
