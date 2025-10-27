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

    private class TestRobot : RobotBase
    {
        public int ActCount { get; private set; }

        public TestRobot() { this.ActCount = 0; }

        public override void Act() => this.ActCount++;
    }

    private class TestScenario : ScenarioBase
    {
        public TestScenario(IMap map, ReadOnlyObservableCollection<IRobot> robots)
            : base(map, robots)
        {
        }

        // Expose a test-only initializer so tests can assign rules after construction.
        public void InitializeRules(ReadOnlyCollection<IRule> rules) => this.SetRules(rules);
    }

    [Fact]
    public void Run_AppliesRuleMultipleTimes_And_CallsRobotActExpectedTimes()
    {
        var map = new FlatMap(3u, 3u);
        var rule = new TestRule(maxApplications: 2); // will apply twice
        var rules = new ReadOnlyCollection<IRule>([rule]);

        var robot = new TestRobot();
        var obs = new ObservableCollection<IRobot> { robot };
        var robots = new ReadOnlyObservableCollection<IRobot>(obs);

        var scenario = new TestScenario(map, robots);
        scenario.InitializeRules(rules);

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
        var rules = new ReadOnlyCollection<IRule>([r1, r2]);

        var robot = new TestRobot();
        var robots = new ReadOnlyObservableCollection<IRobot>([robot]);

        var scenario = new TestScenario(map, robots);
        scenario.InitializeRules(rules);

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
        // Provide a non-empty rules collection (one dummy rule) so SetRules accepts it.
        var rules = new ReadOnlyCollection<IRule>([new TestRule(0)]);

        var robots = new ReadOnlyObservableCollection<IRobot>([new TestRobot()]);

        var scenario = new TestScenario(map, robots);
        scenario.InitializeRules(rules);

        // The Map getter should return the same instance that was provided to the constructor
        Assert.Same(map, scenario.Map);
    }

    [Fact]
    public void Constructor_Throws_OnNullArgumentsOrEmptyRobots()
    {
        var map = new FlatMap(2u, 2u);
        var dummyRule = new TestRule(0);
        var rules = new ReadOnlyCollection<IRule>([dummyRule]);

        var emptyRobots = new ReadOnlyObservableCollection<IRobot>([]);
        Assert.Throws<ArgumentOutOfRangeException>(() => new TestScenario(map, emptyRobots));

        Assert.Throws<ArgumentNullException>(() => new TestScenario(null!, new ReadOnlyObservableCollection<IRobot>([new TestRobot()])));
        var validRobots = new ReadOnlyObservableCollection<IRobot>([new TestRobot()]);
        var s = new TestScenario(map, validRobots);
        Assert.Throws<ArgumentNullException>(() => s.InitializeRules(null!));
        Assert.Throws<ArgumentNullException>(() => new TestScenario(map, null!));
    }

    [Fact]
    public void SetRules_Throws_OnSecondAssignment()
    {
        var map = new FlatMap(2u, 2u);
        var robot = new TestRobot();
        var robots = new ReadOnlyObservableCollection<IRobot>([robot]);

        var scenario = new TestScenario(map, robots);

        var rules1 = new ReadOnlyCollection<IRule>([new TestRule(0)]);
        var rules2 = new ReadOnlyCollection<IRule>([new TestRule(0)]);

        // First assignment ok
        scenario.InitializeRules(rules1);

        // Second assignment should throw InvalidOperationException
        Assert.Throws<InvalidOperationException>(() => scenario.InitializeRules(rules2));
    }
}
