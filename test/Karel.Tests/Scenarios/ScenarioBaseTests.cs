using System.Collections.ObjectModel;
using Xunit;

using Karel.Scenarios;
using Karel.Scenarios.Maps;
using Karel.Scenarios.Rules;
using Karel.Robots;

namespace Karel.Tests.Scenarios;

/// <summary>
/// Tests for the ScenarioBase class.
/// </summary>
public class ScenarioBaseTests
{
    private class TestRule(int maxApplications) : IRule
    {
        private readonly int maxApplications = maxApplications;
        public int AppliedCount { get; private set; } = 0;

        public TimeSpan Duration => TimeSpan.Zero;

        public bool IsApplicable()
        {
            return this.AppliedCount < this.maxApplications;
        }

        public void Initialize() { }

        public void Apply()
        {
            this.AppliedCount++;
        }
    }

    private class TestRobot : IRobot
    {
        public int ActCount { get; private set; }
        public ICell Position { get; } = new Cell(0, 0, 0);

        public TestRobot() { this.ActCount = 0; }

        public void Act() => this.ActCount++;

        public bool TryMoveTo(uint x, uint y, out IList<string> errors) { errors = new List<string>(); return false; }
        public bool TryMoveTo(uint x, uint y, uint z, out IList<string> errors) { errors = new List<string>(); return false; }
    }

    private class TestScenario : ScenarioBase
    {
        public TestScenario(IMap map, ReadOnlyCollection<IRule> rules, ReadOnlyObservableCollection<IRobot> robots)
            : base(map, rules, robots)
        {
        }
    }

    [Fact]
    public void Run_AppliesRuleMultipleTimes_And_CallsRobotActExpectedTimes()
    {
        var map = new FlatMap(3u, 3u);
        var rule = new TestRule(maxApplications: 2); // will apply twice
        var rules = new ReadOnlyCollection<IRule>(new List<IRule> { rule });

        var robot = new TestRobot();
        var obs = new ObservableCollection<IRobot> { robot };
        var robots = new ReadOnlyObservableCollection<IRobot>(obs);

        var scenario = new TestScenario(map, rules, robots);

        scenario.Run();

        // Rule applied exactly twice
        Assert.Equal(2, rule.AppliedCount);

        // Robot acted maxApplications + 1 times (final iteration where rule no longer applies still runs robots)
        Assert.Equal(3, robot.ActCount);
    }

    [Fact]
    public void Run_AppliesAllApplicableRules_EachIteration()
    {
        var map = new FlatMap(3u, 3u);
        var r1 = new TestRule(1);
        var r2 = new TestRule(1);
        var rules = new ReadOnlyCollection<IRule>(new List<IRule> { r1, r2 });

        var robot = new TestRobot();
        var robots = new ReadOnlyObservableCollection<IRobot>(new ObservableCollection<IRobot> { robot });

        var scenario = new TestScenario(map, rules, robots);

        scenario.Run();

        Assert.Equal(1, r1.AppliedCount);
        Assert.Equal(1, r2.AppliedCount);
        // both applied in first iteration, then stop -> robots ran twice
        Assert.Equal(2, robot.ActCount);
    }

    [Fact]
    public void Constructor_MapGetter_ReturnsConstructorMap()
    {
        var map = new FlatMap(2u, 2u);
        var rules = new ReadOnlyCollection<IRule>(new List<IRule>());

        var robots = new ReadOnlyObservableCollection<IRobot>(new ObservableCollection<IRobot> { new TestRobot() });

        var scenario = new TestScenario(map, rules, robots);

        // The Map getter should return the same instance that was provided to the constructor
        Assert.Same(map, scenario.Map);
    }

    [Fact]
    public void Constructor_Throws_OnNullArgumentsOrEmptyRobots()
    {
        var map = new FlatMap(1u, 1u);
        var dummyRule = new TestRule(0);
        var rules = new ReadOnlyCollection<IRule>(new List<IRule> { dummyRule });

        var emptyRobots = new ReadOnlyObservableCollection<IRobot>(new ObservableCollection<IRobot>());
        Assert.Throws<ArgumentOutOfRangeException>(() => new TestScenario(map, rules, emptyRobots));

        Assert.Throws<ArgumentNullException>(() => new TestScenario(null!, rules, new ReadOnlyObservableCollection<IRobot>(new ObservableCollection<IRobot> { new TestRobot() })));
        Assert.Throws<ArgumentNullException>(() => new TestScenario(map, null!, new ReadOnlyObservableCollection<IRobot>(new ObservableCollection<IRobot> { new TestRobot() })));
        Assert.Throws<ArgumentNullException>(() => new TestScenario(map, rules, null!));
    }
}
