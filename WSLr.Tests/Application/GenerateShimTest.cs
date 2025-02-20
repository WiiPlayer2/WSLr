using FluentAssertions;
using LanguageExt.Sys.Test;
using WSLr.Application;
using WSLr.Domain;

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
        var runtime = Runtime.New();

        // Act
        var result = await subject.With(shimBinary).Run(runtime);

        // Assert
        (result as IComparable<Fin<Unit>>).Should().Be(unit);
    }

    private GenerateShim<Runtime> CreateSubject()
    {
        return new();
    }
}
