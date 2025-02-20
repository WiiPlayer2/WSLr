using System;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace WSLr.Tests.Extensions;

internal class FinAssertions<T>(Fin<T> value, AssertionChain assertionChain)
    : ObjectAssertions<Fin<T>, FinAssertions<T>>(value, assertionChain)
{
    protected override string Identifier => "fin";

    public AndWhichConstraint<FinAssertions<T>, T> BeSuccess(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsSucc)
            .FailWith("Expected {context:fin} to be successful{reason}, but it failed.");

        return new AndWhichConstraint<FinAssertions<T>, T>(this, (T) Subject, CurrentAssertionChain);
    }

    public AndConstraint<FinAssertions<T>> BeFail(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsFail)
            .FailWith("Expected {context:fin} to be failed{reason}, but found {0}.", Subject);
        
        return new AndConstraint<FinAssertions<T>>(this);
    }
}
