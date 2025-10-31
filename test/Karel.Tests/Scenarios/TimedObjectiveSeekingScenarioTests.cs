using System.Collections.ObjectModel;
using Xunit;
using Karel.Scenarios;
using Karel.Scenarios.Maps;
using Karel.Robots;

namespace Karel.Tests.Scenarios;

public class TimedObjectiveSeekingScenarioTests
{
    [Fact]
    public void Constructor_Default_CreatesFlatMapAndTwoRobots()
    {
        var duration = TimeSpan.FromSeconds(1);
        var scenario = new TimedObjectiveSeekingScenario(3u, 3u, duration);

        Assert.IsType<FlatMap>(scenario.Map);
        Assert.Equal(2, scenario.Robots.Count);
        Assert.All(scenario.Robots, r => Assert.IsType<ObjectiveSeekingRobot>(r));
    }

    [Fact]
    public void Run_WithLongDuration_RunsRobotsOnce()
    {
        // Arrange: inject test robots that count Act() calls
        var map = new TestMap(3u, 3u);
        var r1 = new CountingRobot();
        var r2 = new CountingRobot();
        var obs = new ObservableCollection<IRobot> { r1, r2 };
        var robots = new ReadOnlyObservableCollection<IRobot>(obs);

        var scenario = new TimedObjectiveSeekingScenario(map, robots, TimeSpan.FromSeconds(1));

        // Act
        scenario.Run();

        // Assert: each robot acted at least once (rules not applicable)
        Assert.True(r1.Count >= 1);
        Assert.True(r2.Count >= 1);
    }

    private sealed class TestMap(uint width, uint height) : GridMapBase(width, height, 1)
    {
    }

    private sealed class CountingRobot : RobotBase
    {
        public int Count { get; private set; }
        public override void Act() => Count++;
    }
}

