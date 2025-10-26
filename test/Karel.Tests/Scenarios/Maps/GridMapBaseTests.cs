using Xunit;

using Karel.Scenarios.Maps;

namespace Karel.Tests.Scenarios.Maps;

/// <summary>
/// Tests for the GridMapBase class.
/// </summary>
public sealed class GridMapBaseTests
{
    private class ConcreteGridMap(uint width, uint height, uint depth) : GridMapBase(width, height, depth) { }

    [Fact]
    public void TryGetCell_ReturnsTrueAndCell_WhenInBounds()
    {
        var map = new FlatMap(5u, 5u);

        var success = map.TryGetCell(0u, 0u, 0u, out var cell);

        Assert.True(success);
        Assert.NotNull(cell);
        Assert.Equal(0u, cell!.X);
        Assert.Equal(0u, cell.Y);
        Assert.Equal(0u, cell.Z);
    }

    [Fact]
    public void TryGetCell_ReturnsFalse_WhenOutOfBounds()
    {
        var map = new FlatMap(3u, 3u);

        var success = map.TryGetCell(map.Width, 0u, 0u, out var cell);

        Assert.False(success);
        Assert.Null(cell);
    }

    [Fact]
    public void TryGetCell_ReturnsFalse_WhenOutOfBounds_GetCellRemoved()
    {
        var map = new FlatMap(3u, 3u);

        var success = map.TryGetCell(map.Width, 0u, 0u, out var cell);
        Assert.False(success);
        Assert.Null(cell);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ReturnsFourForCenterCell_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);

        var neighbors = map.GetOrthogonalNeighbors(2u, 2u, 0u).ToList();

        // On a flat map with depth == 1, only the four orthogonal neighbors in X/Y exist.
        Assert.Equal(4, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
            {
                (3u, 2u, 0u),
                (1u, 2u, 0u),
                (2u, 3u, 0u),
                (2u, 1u, 0u)
            };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ReturnsCorrectCountForCorner_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);

        var neighbors = map.GetOrthogonalNeighbors(0u, 0u, 0u).ToList();

        // Corner (0,0) has two orthogonal neighbors in a 2D grid when depth==1
        Assert.Equal(2, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)> { (1u, 0u, 0u), (0u, 1u, 0u) };
        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_ReturnsEightForCenterCell_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);

        var neighbors = map.GetAllAdjacentNeighbors(2u, 2u, 0u).ToList();

        // On a flat map (depth == 1) the 3x3x3 neighborhood collapses to the 3x3 plane -> 8 neighbors
        Assert.Equal(8, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>();
        for (uint dx = 1u; dx <= 3u; dx++)
            for (uint dy = 1u; dy <= 3u; dy++)
            {
                if (dx == 2u && dy == 2u) continue;
                expected.Add((dx, dy, 0u));
            }

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ReturnsSixForCenterCell_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);

        var neighbors = map.GetOrthogonalNeighbors(1u, 1u, 1u).ToList();

        Assert.Equal(6, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
            {
                (2u,1u,1u),(0u,1u,1u),(1u,2u,1u),(1u,0u,1u),(1u,1u,2u),(1u,1u,0u)
            };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_Returns26ForCenterCell_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);

        var neighbors = map.GetAllAdjacentNeighbors(1u, 1u, 1u).ToList();

        Assert.Equal(26, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>();
        for (uint dx = 0u; dx <= 2u; dx++)
            for (uint dy = 0u; dy <= 2u; dy++)
                for (uint dz = 0u; dz <= 2u; dz++)
                {
                    if (dx == 1u && dy == 1u && dz == 1u) continue;
                    expected.Add((dx, dy, dz));
                }

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ReturnsThreeForCorner_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);

        var neighbors = map.GetOrthogonalNeighbors(0u, 0u, 0u).ToList();

        // Corner (0,0,0) in 3D has three orthogonal neighbors (+X, +Y, +Z)
        Assert.Equal(3, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)> { (1u, 0u, 0u), (0u, 1u, 0u), (0u, 0u, 1u) };
        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_ReturnsSevenForCorner_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);

        var neighbors = map.GetAllAdjacentNeighbors(0u, 0u, 0u).ToList();

        // Corner 3D neighborhood (2^3 - 1) = 7
        Assert.Equal(7, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>();
        for (uint dx = 0u; dx <= 1u; dx++)
            for (uint dy = 0u; dy <= 1u; dy++)
                for (uint dz = 0u; dz <= 1u; dz++)
                {
                    if (dx == 0u && dy == 0u && dz == 0u) continue;
                    expected.Add((dx, dy, dz));
                }

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_ICell_ReturnsEightForCenter_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);
        Assert.True(map.TryGetCell(2u, 2u, 0u, out var cell));
        var neighbors = map.GetAllAdjacentNeighbors(cell!).ToList();

        Assert.Equal(8, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>();
        for (uint dx = 1u; dx <= 3u; dx++)
            for (uint dy = 1u; dy <= 3u; dy++)
            {
                if (dx == 2u && dy == 2u) continue;
                expected.Add((dx, dy, 0u));
            }

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_ICell_ReturnsThreeForCorner_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);
        Assert.True(map.TryGetCell(0u, 0u, 0u, out var cell));
        var neighbors = map.GetAllAdjacentNeighbors(cell!).ToList();

        Assert.Equal(3, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
        {
            (0u, 1u, 0u),
            (1u, 0u, 0u),
            (1u, 1u, 0u)
        };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_ICell_ReturnsZero_OnSingleCellFlatMap()
    {
        var map = new FlatMap(1u, 1u);
        Assert.True(map.TryGetCell(0u, 0u, 0u, out var cell));

        var neighbors = map.GetAllAdjacentNeighbors(cell!).ToList();

        Assert.Empty(neighbors);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_ICell_Returns26ForCenter_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);
        Assert.True(map.TryGetCell(1u, 1u, 1u, out var cell));

        var neighbors = map.GetAllAdjacentNeighbors(cell!).ToList();

        Assert.Equal(26, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>();
        for (uint dx = 0u; dx <= 2u; dx++)
            for (uint dy = 0u; dy <= 2u; dy++)
                for (uint dz = 0u; dz <= 2u; dz++)
                {
                    if (dx == 1u && dy == 1u && dz == 1u) continue;
                    expected.Add((dx, dy, dz));
                }

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetAllAdjacentNeighbors_ICell_ReturnsSevenForCorner_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);
        Assert.True(map.TryGetCell(0u, 0u, 0u, out var cell));

        var neighbors = map.GetAllAdjacentNeighbors(cell!).ToList();

        Assert.Equal(7, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>();
        for (uint dx = 0u; dx <= 1u; dx++)
            for (uint dy = 0u; dy <= 1u; dy++)
                for (uint dz = 0u; dz <= 1u; dz++)
                {
                    if (dx == 0u && dy == 0u && dz == 0u) continue;
                    expected.Add((dx, dy, dz));
                }

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ICell_ReturnsFourForCenter_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);
        Assert.True(map.TryGetCell(2u, 2u, 0u, out var cell));

        var neighbors = map.GetOrthogonalNeighbors(cell!).ToList();

        Assert.Equal(4, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
        {
            (1u, 2u, 0u), // West
            (3u, 2u, 0u), // East
            (2u, 1u, 0u), // North
            (2u, 3u, 0u)  // South
        };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ICell_ReturnsTwoForCorner_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);
        Assert.True(map.TryGetCell(0u, 0u, 0u, out var cell));

        var neighbors = map.GetOrthogonalNeighbors(cell!).ToList();

        Assert.Equal(2, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
        {
            (1u, 0u, 0u),
            (0u, 1u, 0u)
        };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ICell_ReturnsThreeForEdge_OnFlatMap()
    {
        var map = new FlatMap(5u, 5u);
        Assert.True(map.TryGetCell(0u, 2u, 0u, out var cell));

        var neighbors = map.GetOrthogonalNeighbors(cell!).ToList();

        Assert.Equal(3, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
        {
            (1u, 2u, 0u), // East
            (0u, 1u, 0u), // North
            (0u, 3u, 0u)  // South
        };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ICell_ReturnsZero_OnSingleCellFlatMap()
    {
        var map = new FlatMap(1u, 1u);
        Assert.True(map.TryGetCell(0u, 0u, 0u, out var cell));

        var neighbors = map.GetOrthogonalNeighbors(cell!).ToList();

        Assert.Empty(neighbors);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ICell_ReturnsSixForCenter_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);
        Assert.True(map.TryGetCell(1u, 1u, 1u, out var cell));

        var neighbors = map.GetOrthogonalNeighbors(cell!).ToList();

        Assert.Equal(6, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
        {
            (0u, 1u, 1u),
            (2u, 1u, 1u),
            (1u, 0u, 1u),
            (1u, 2u, 1u),
            (1u, 1u, 0u),
            (1u, 1u, 2u)
        };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ICell_ReturnsThreeForCorner_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);
        Assert.True(map.TryGetCell(0u, 0u, 0u, out var cell));

        var neighbors = map.GetOrthogonalNeighbors(cell!).ToList();

        Assert.Equal(3, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
        {
            (1u, 0u, 0u),
            (0u, 1u, 0u),
            (0u, 0u, 1u)
        };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void GetOrthogonalNeighbors_ICell_ReturnsFiveForFaceCenter_In3D()
    {
        var map = new ConcreteGridMap(3u, 3u, 3u);
        Assert.True(map.TryGetCell(1u, 1u, 0u, out var cell));

        var neighbors = map.GetOrthogonalNeighbors(cell!).ToList();

        Assert.Equal(5, neighbors.Count);

        var coords = neighbors.Select(n => (n.X, n.Y, n.Z)).ToHashSet();
        var expected = new HashSet<(uint, uint, uint)>
        {
            (0u, 1u, 0u),
            (2u, 1u, 0u),
            (1u, 0u, 0u),
            (1u, 2u, 0u),
            (1u, 1u, 1u)
        };

        Assert.Equal(expected, coords);
    }

    [Fact]
    public void InBounds_WithValidCoordinates_ReturnsTrue()
    {
        var map = new ConcreteGridMap(3, 3, 3);
        Assert.True(map.InBounds(0, 0, 0));
        Assert.True(map.InBounds(2, 2, 2));
        Assert.True(map.InBounds(1, 1, 1));
    }

    [Fact]
    public void InBounds_WithInvalidCoordinates_ReturnsFalse()
    {
        var map = new ConcreteGridMap(3, 3, 3);
        Assert.False(map.InBounds(3, 0, 0));
        Assert.False(map.InBounds(0, 3, 0));
        Assert.False(map.InBounds(0, 0, 3));
        Assert.False(map.InBounds(100, 100, 100));
    }

    [Fact]
    public void InBounds_ICell_WithinBounds_ReturnsTrue()
    {
        var map = new ConcreteGridMap(2, 2, 2);
        var cell = new Cell(1, 1, 1, map);
        Assert.True(map.InBounds(cell));
    }

    [Fact]
    public void InBounds_ICell_OutOfBounds_ReturnsFalse()
    {
        var map = new ConcreteGridMap(2, 2, 2);
        var cell = new Cell(2, 0, 0, map);
        Assert.False(map.InBounds(cell));
    }

    [Fact]
    public void InBounds_ICell_Null_ThrowsArgumentNullException()
    {
        var map = new ConcreteGridMap(2, 2, 2);
        Assert.Throws<ArgumentNullException>(() => map.InBounds(null!));
    }
}
