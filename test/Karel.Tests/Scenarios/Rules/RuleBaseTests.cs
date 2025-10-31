using Xunit;
using Karel.Scenarios.Rules;
using Karel.Scenarios.Maps;
using Karel.Robots;

namespace Karel.Tests.Scenarios.Rules;

public class RuleBaseTests
{
    private sealed class FakeMap() : GridMapBase(2u, 2u, 1u) { }

    private class TestRobot : RobotBase
    {
        public override void Act() { }
    }

    private class TestRuleNoOverride : RuleBase
    {
        // inherits Apply which throws by default
    }

    private class TestConcreteRule : RuleBase
    {
        public int AppliedCount { get; private set; }
        public override bool IsApplicable() => true;
        public override void Apply() => this.AppliedCount++;
    }

    [Fact]
    public void Apply_OnBase_Throws_NotImplemented()
    {
        var rule = new TestRuleNoOverride();

        // Initialize and IsApplicable should not throw
        rule.Initialize();
        Assert.False(rule.IsApplicable());

        // But Apply uses base implementation which throws NotImplementedException
        Assert.Throws<NotImplementedException>(() => rule.Apply());
    }
}

