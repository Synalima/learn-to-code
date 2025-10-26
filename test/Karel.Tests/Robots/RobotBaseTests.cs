using Xunit;

using Karel.Robots;
using Karel.Scenarios.Maps;

namespace Karel.Tests.Robots;

public class RobotBaseTests
{
    private class FakeMap(uint width, uint height, uint depth) : GridMapBase(width, height, depth) { }

    private class ConcreteRobot : RobotBase
    {
        public ConcreteRobot() : base() { }
        public override void Act() { }
    }

    [Fact]
    public void TryMoveTo_TargetCellDoesNotExist_ReturnsFalseAndError()
    {
        var map = new FakeMap(3, 3, 1);
        var start = new Cell(2, 2, 0, map);
      
        var robot = new ConcreteRobot();
        robot.Initialize(start);

        var ok = robot.TryMoveTo(5, 5, out var errors);

        Assert.False(ok);
        Assert.Contains(errors, e => e.Contains("does not exist", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void TryMoveTo_TargetCellNotNavigable_ReturnsFalseAndError()
    {
        var map = new FakeMap(3, 3, 1);
        var start = new Cell(0, 0, 0, map);
        if(map.TryGetCell(1, 0, 0, out var target))
        {
            target!.Type = CellType.Obstructed;
        }

        var robot = new ConcreteRobot();
        robot.Initialize(start);

        var ok = robot.TryMoveTo(1, 0, out var errors);

        Assert.False(ok);
        Assert.Contains(errors, e => e.Contains("not navigable", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void TryMoveTo_TargetNotAdjacent_ReturnsFalseAndError()
    {
        var map = new FakeMap(4, 4, 1);
        var start = new Cell(0, 0, 0, map);
        var target = new Cell(2, 2, 0, map, CellType.Empty);

        var robot = new ConcreteRobot();
        robot.Initialize(start);

        var ok = robot.TryMoveTo(2, 2, out var errors);

        Assert.False(ok);
        Assert.Contains(errors, e => e.Contains("not adjacent", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void TryMoveTo_Succeeds_WhenTargetIsAdjacentAndNavigable()
    {
        var map = new FakeMap(3, 3, 1);
        var start = new Cell(1, 1, 0, map);
        var target = new Cell(2, 1, 0, map, CellType.Empty);

        var robot = new ConcreteRobot();
        robot.Initialize(start);

        var ok = robot.TryMoveTo(2, 1, out var errors);

        Assert.True(ok);
        Assert.Empty(errors);
        Assert.Equal(target.X, robot.Position.X);
        Assert.Equal(target.Y, robot.Position.Y);
        Assert.Equal(target.Z, robot.Position.Z);
    }

    [Fact]
    public void Position_Set_RaisesPropertyChanged_WithOldAndNewValues()
    {
        var map = new FakeMap(3, 3, 1);
        var start = new Cell(1, 1, 0, map);
        var target = new Cell(2, 1, 0, map, CellType.Empty);

        var robot = new ConcreteRobot();
        robot.Initialize(start);

        bool raised = false;
        Karel.Common.PropertyChangedEventArgs<ICell>? received = null;
        robot.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(robot.Position) && args is Karel.Common.PropertyChangedEventArgs<ICell> pa)
            {
                raised = true;
                received = pa;
            }
        };

        robot.Position = target;

        Assert.True(raised);
        Assert.NotNull(received);
        Assert.Equal(start, received!.OldValue);
        Assert.Equal(target, received.NewValue);
    }

    [Fact]
    public void TryMoveTo_WhenMapThrows_ReturnsFalseAndContainsExceptionMessage()
    {
        var map = new FakeMap(1, 1, 1);
        var start = new Cell(0, 0, 0, map);
        var robot = new ConcreteRobot();
        robot.Initialize(start);

        var ok = robot.TryMoveTo(2, 2, out var errors);

        Assert.False(ok);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void TryMoveTo_WhenRobotNotInitializedThrows_ReturnsFalseAndContainsExceptionMessage()
    {
        var robot = new ConcreteRobot();

        var ok = robot.TryMoveTo(2, 2, out var errors);

        Assert.False(ok);
        Assert.NotEmpty(errors);
    }    
}
