using LanguageExt.Effects.Traits;

namespace WSLr.Application;

public class GenerateShim<RT>
    where RT : struct, HasCancel<RT>
{
    public Aff<RT, Unit> With(ShimBinary shimBinary) => unitAff;
}
