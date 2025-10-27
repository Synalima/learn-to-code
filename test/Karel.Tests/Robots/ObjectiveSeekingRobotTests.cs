using Xunit;
using Karel.Robots;
using Karel.Scenarios.Maps;

namespace Karel.Tests.Robots;

public class ObjectiveSeekingRobotTests
{
    private sealed class TestMap(uint width, uint height) : GridMapBase(width, height, 1) { }

    private sealed class TestMap3D : GridMapBase
    {
        public TestMap3D(uint width, uint height, uint depth) : base(width, height, depth) { }
    }

    private sealed class FakeMap : IMap
    {
        public uint Width { get; }
        public uint Height { get; }
        public uint Depth { get; }

        private readonly Dictionary<(uint, uint, uint), Cell> cells = new();
        private readonly HashSet<(uint, uint, uint)> failingCoordinates = new();

        public FakeMap(uint width, uint height, uint depth, params (uint, uint, uint)[] failCoords)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;

            if (failCoords is not null)
            {
                foreach (var t in failCoords) this.failingCoordinates.Add(t);
            }

            for (uint z = 0; z < depth; z++)
                for (uint y = 0; y < height; y++)
                    for (uint x = 0; x < width; x++)
                        this.cells[(x, y, z)] = new Cell(x, y, z, this);
        }

        public bool InBounds(ICell cell)
        {
            ArgumentNullException.ThrowIfNull(cell);
            return InBounds(cell.X, cell.Y, cell.Z);
        }

        public bool InBounds(uint x, uint y, uint z) => x < Width && y < Height && z < Depth;

        public bool TryGetCell(uint x, uint y, uint z, out Cell? cell)
        {
            if (!InBounds(x, y, z))
            {
                cell = null;
                return false;
            }

            if (this.failingCoordinates.Contains((x, y, z)))
            {
                cell = null;
                return false;
            }

            cell = this.cells[(x, y, z)];
            return true;
        }

        public IEnumerable<ICell> GetOrthogonalNeighbors(ICell cell)
        {
            ArgumentNullException.ThrowIfNull(cell);
            return GetOrthogonalNeighbors(cell.X, cell.Y, cell.Z);
        }

        public IEnumerable<ICell> GetOrthogonalNeighbors(uint x, uint y, uint z)
        {
            var deltas = new (int dx, int dy, int dz)[] { (1,0,0), (-1,0,0), (0,1,0), (0,-1,0), (0,0,1), (0,0,-1) };
            foreach (var (dx,dy,dz) in deltas)
            {
                long nx = x + dx; long ny = y + dy; long nz = z + dz;
                if (nx >= 0 && ny >= 0 && nz >= 0 && (uint)nx < Width && (uint)ny < Height && (uint)nz < Depth)
                {
                    if (TryGetCell((uint)nx, (uint)ny, (uint)nz, out var c) && c is not null)
                        yield return c;
                }
            }
        }

        public IEnumerable<ICell> GetAllAdjacentNeighbors(ICell cell)
        {
            ArgumentNullException.ThrowIfNull(cell);
            for (int dz = -1; dz <= 1; dz++)
                for (int dy = -1; dy <= 1; dy++)
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0 && dz == 0) continue;
                        long nx = cell.X + dx; long ny = cell.Y + dy; long nz = cell.Z + dz;
                        if (nx >= 0 && ny >= 0 && nz >= 0 && (uint)nx < Width && (uint)ny < Height && (uint)nz < Depth)
                        {
                            if (TryGetCell((uint)nx, (uint)ny, (uint)nz, out var c) && c is not null)
                                yield return c;
                        }
                    }
        }

        public IEnumerable<ICell> GetAllAdjacentNeighbors(uint x, uint y, uint z)
        {
            for (int dz = -1; dz <= 1; dz++)
                for (int dy = -1; dy <= 1; dy++)
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0 && dz == 0) continue;
                        long nx = x + dx; long ny = y + dy; long nz = z + dz;
                        if (nx >= 0 && ny >= 0 && nz >= 0 && (uint)nx < Width && (uint)ny < Height && (uint)nz < Depth)
                        {
                            if (TryGetCell((uint)nx, (uint)ny, (uint)nz, out var c) && c is not null)
                                yield return c;
                        }
                    }
        }
    }

    [Fact]
    public void Act_MovesToAdjacentObjective()
    {
        // Arrange: 2x1 map, start at (0,0,0), objective at (1,0,0)
        var map = new TestMap(2u, 2u);
        Assert.True(map.TryGetCell(0, 0, 0, out var start));
        Assert.True(map.TryGetCell(1, 0, 0, out var objective));

        // Set types explicitly
        start!.Type = CellType.Empty;
        objective!.Type = CellType.Objective | CellType.Empty;

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        // Act
        robot.Act();

        // Assert: robot moved to objective
        Assert.Equal(objective, robot.Position);
    }

    [Fact]
    public void Act_MovesOneStepTowardObjective_WhenFar()
    {
        // Arrange: 3x1 map, start at (0,0,0), objective at (2,0,0)
        var map = new TestMap(3u, 2u);
        Assert.True(map.TryGetCell(0, 0, 0, out var start));
        Assert.True(map.TryGetCell(1, 0, 0, out var middle));
        Assert.True(map.TryGetCell(2, 0, 0, out var objective));

        start!.Type = CellType.Empty;
        middle!.Type = CellType.Empty;
        objective!.Type = CellType.Objective | CellType.Empty;

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        // Act
        robot.Act();

        // Assert: robot moved one step to the middle cell
        Assert.Equal(middle, robot.Position);
    }

    [Fact]
    public void Act_DoesNothing_WhenNoObjectivePresent()
    {
        // Arrange: simple 2x2 map with no objective
        var map = new TestMap(2u, 2u);
        Assert.True(map.TryGetCell(0, 0, 0, out var start));

        start!.Type = CellType.Empty;

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        // Act
        robot.Act();

        // Assert: position unchanged
        Assert.Equal(start, robot.Position);
    }

    [Fact]
    public void Act_DoesNothing_WhenNotInitialized()
    {
        var robot = new ObjectiveSeekingRobot();
        // should not throw and position remains null
        robot.Act();
        Assert.Null(robot.Position);
    }

    [Fact]
    public void Act_DoesNothing_WhenAlreadyAtObjective()
    {
        var map = new TestMap(2u, 2u);
        Assert.True(map.TryGetCell(0, 0, 0, out var start));
        start!.Type = CellType.Objective | CellType.Empty;

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        robot.Act();

        Assert.Equal(start, robot.Position);
    }

    [Fact]
    public void Act_DoesNotMove_WhenAllAdjacentObstructed()
    {
        // 3x3 map, place robot in corner (0,0) and make all its adjacent neighbors obstructed
        var map = new TestMap(3u, 3u);
        Assert.True(map.TryGetCell(0, 0, 0, out var start));
        start!.Type = CellType.Empty;

        // put objective somewhere not adjacent to the start (2,2)
        Assert.True(map.TryGetCell(2, 2, 0, out var objective));
        objective!.Type = CellType.Objective | CellType.Empty;

        // make all neighbors of the start obstructed (these do not include the objective)
        foreach (var nb in map.GetAllAdjacentNeighbors(start))
        {
            if (nb is Cell c)
            {
                c.Type = CellType.Obstructed;
            }
        }

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        robot.Act();

        // robot should not move because neighbors are not navigable
        Assert.Equal(start, robot.Position);
    }

    [Fact]
    public void Act_DoesNotMove_WhenAdjacentObjectiveIsObstructed()
    {
        var map = new TestMap(2u, 2u);
        Assert.True(map.TryGetCell(0, 0, 0, out var start));
        Assert.True(map.TryGetCell(1, 0, 0, out var objective));

        start!.Type = CellType.Empty;
        // objective marked as objective but also obstructed
        objective!.Type = CellType.Objective | CellType.Obstructed;

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        robot.Act();

        // Act will attempt TryMoveTo but it should fail because target is obstructed
        Assert.Equal(start, robot.Position);
    }

    [Fact]
    public void Act_MovesToObjective_In3D()
    {
        var map = new TestMap3D(2u, 2u, 2u);
        Assert.True(map.TryGetCell(0, 0, 0, out var start));
        Assert.True(map.TryGetCell(0, 0, 1, out var objective));

        start!.Type = CellType.Empty;
        objective!.Type = CellType.Objective | CellType.Empty;

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        robot.Act();

        // objective is adjacent in Z, robot should move there
        Assert.Equal(objective, robot.Position);
    }

    [Fact]
    public void Act_FindsObjective_WhenTryGetCellSometimesFails()
    {
        // Arrange: FakeMap where TryGetCell returns false for (0,0,0) but true for (1,0,0)
        var map = new FakeMap(3u, 1u, 1u, (0u, 0u, 0u));

        // Mark objective at (1,0,0)
        Assert.True(map.TryGetCell(1, 0, 0, out var objective));
        objective!.Type = CellType.Objective | CellType.Empty;

        // Create a start cell referencing this fake map. Note: map.TryGetCell will return false for start coords,
        // but Initialize only requires InBounds to be true which FakeMap provides.
        var start = new Cell(0, 0, 0, map, CellType.Empty);

        var robot = new ObjectiveSeekingRobot();
        robot.Initialize(start);

        // Act
        robot.Act();

        // Assert: robot should move to the objective despite TryGetCell returning false for start coordinates
        Assert.Equal(objective, robot.Position);
    }
}

