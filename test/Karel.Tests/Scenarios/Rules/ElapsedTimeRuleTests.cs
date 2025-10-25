using System.Collections.ObjectModel;
using Xunit;
using Karel.Scenarios.Rules;
using Karel.Scenarios.Maps;
using Karel.Robots;

namespace Karel.Tests.Scenarios.Rules;

/// <summary>
/// Tests for the ElapsedTimeRule class.
/// </summary>
public class ElapsedTimeRuleTests
{
    private class TestRobot : IRobot
    {
        public ICell Position { get; } = new Cell(0, 0, 0);
        public void Act() { }
        public bool TryMoveTo(uint x, uint y, out IList<string> errors) { errors = []; return false; }
        public bool TryMoveTo(uint x, uint y, uint z, out IList<string> errors) { errors = []; return false; }
    }

    [Fact]
    public void IsApplicable_IsFalse_BeforeInitialize()
    {
        var map = new FlatMap(1u, 1u);
        var robots = new ReadOnlyCollection<IRobot>(new List<IRobot> { new TestRobot() });
        var rule = new ElapsedTimeRule(map, robots, TimeSpan.FromMilliseconds(10));

        Assert.False(rule.IsApplicable());
    }

    [Fact]
    public async Task IsApplicable_BecomesTrue_AfterDurationFromInitialize()
    {
        var map = new FlatMap(1u, 1u);
        var robots = new ReadOnlyCollection<IRobot>(new List<IRobot> { new TestRobot() });
        var rule = new ElapsedTimeRule(map, robots, TimeSpan.FromMilliseconds(50));

        rule.Initialize();
        
        // Immediately after initialize it should not be applicable
        Assert.False(rule.IsApplicable());

        // Wait long enough for the duration to elapse
        await Task.Delay(100);

        Assert.True(rule.IsApplicable());
    }
}
