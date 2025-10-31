using Xunit;
using Karel.Scenarios.Rules;

namespace Karel.Tests.Scenarios.Rules;

/// <summary>
/// Tests for the ElapsedTimeRule class.
/// </summary>
public class ElapsedTimeRuleTests
{
    [Fact]
    public void IsApplicable_IsFalse_BeforeInitialize()
    {
        var rule = new ElapsedTimeRule(TimeSpan.FromMilliseconds(10));

        Assert.False(rule.IsApplicable());
    }

    [Fact]
    public async Task IsApplicable_BecomesTrue_AfterDurationFromInitialize()
    {
        var rule = new ElapsedTimeRule(TimeSpan.FromMilliseconds(50));

        rule.Initialize();

        Assert.True(rule.IsApplicable());

        // Wait long enough for the duration to elapse
        await Task.Delay(100);

        Assert.False(rule.IsApplicable());
    }
}
