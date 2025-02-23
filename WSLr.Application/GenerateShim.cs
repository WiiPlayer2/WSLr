﻿using LanguageExt.Effects.Traits;

namespace WSLr.Application;

public class GenerateShim<RT>(IOutputWriter<RT> outputWriter, IShimBuilder<RT> shimBuilder)
    where RT : struct, HasCancel<RT>
{
    public Aff<RT, Unit> With(ShimTarget shimTarget, Option<ShimFixStreamLineEndings> shimFixStreamLineEndings = default) =>
        from buildConfig in SuccessAff(new ShimBuildConfig(
            shimTarget,
            shimFixStreamLineEndings.IfNone(ShimFixStreamLineEndings.From(false))))
        let outputPath = GetOutputFilename.With(shimTarget)
        from outputData in shimBuilder.Build(buildConfig)
        from _ in outputWriter.Write(outputPath, outputData)
        select unit;
}
