namespace Karel.Scenarios.Maps;

/// <summary>
/// A grid-based map for Karel the Robot.
/// </summary>
public abstract class GridMapBase : IMap
{
    private readonly Cell[,,] cells;

    /// <inheritdoc/>
    public uint Width { get; }

    /// <inheritdoc/>
    public uint Height { get; }

    /// <inheritdoc/>
    public uint Depth { get; }

    /// <inheritdoc/>
    public bool InBounds(ICell cell)
    {
        ArgumentNullException.ThrowIfNull(cell);

        return this.InBounds(cell.X, cell.Y, cell.Z);
    }

    /// <inheritdoc/>
    public bool InBounds(uint x, uint y, uint z) => x < this.Width && y < this.Height && z < this.Depth;

    /// <inheritdoc/>
    public bool TryGetCell(uint x, uint y, uint z, out Cell? cell)
    {
        if (InBounds(x, y, z))
        {
            cell = this.cells[x, y, z];
            return true;
        }

        cell = null;
        return false;
    }

    /// <inheritdoc/>
    public IEnumerable<ICell> GetOrthogonalNeighbors(ICell cell)
    {
        ArgumentNullException.ThrowIfNull(cell);

        return this.GetOrthogonalNeighbors(cell.X, cell.Y, cell.Z);
    }

    /// <inheritdoc/>
    public IEnumerable<ICell> GetOrthogonalNeighbors(uint x, uint y, uint z)
    {
        // Offsets for the 6 axis-aligned neighbors.
        var deltas = new (int dx, int dy, int dz)[]
        {
            (1, 0, 0), (-1, 0, 0),
            (0, 1, 0), (0, -1, 0),
            (0, 0, 1), (0, 0, -1)
        };

        foreach (var (dx, dy, dz) in deltas)
        {
            long nx = x + dx;
            long ny = y + dy;
            long nz = z + dz;

            if (nx >= 0 && ny >= 0 && nz >= 0
                && (uint)nx < this.Width && (uint)ny < this.Height && (uint)nz < this.Depth)
            {
                yield return this.cells[(uint)nx, (uint)ny, (uint)nz];
            }
        }
    }

    /// <inheritdoc/>
    public IEnumerable<ICell> GetAllAdjacentNeighbors(ICell cell)
    {
        ArgumentNullException.ThrowIfNull(cell);

        return this.GetAllAdjacentNeighbors(cell.X, cell.Y, cell.Z);
    }

    /// <inheritdoc/>
    public IEnumerable<ICell> GetAllAdjacentNeighbors(uint x, uint y, uint z)
    {
        for (int dz = -1; dz <= 1; dz++)
            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0 && dz == 0)
                    {
                        continue; // skip the center
                    }

                    long nx = x + dx;
                    long ny = y + dy;
                    long nz = z + dz;

                    if (nx >= 0 && ny >= 0 && nz >= 0
                        && (uint)nx < this.Width && (uint)ny < this.Height && (uint)nz < this.Depth)
                    {
                        yield return this.cells[(uint)nx, (uint)ny, (uint)nz];
                    }
                }
    }

    /// <summary>
    /// Creates a new GridMap with the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the map.</param>
    /// <param name="height">The height of the map.</param>
    /// <param name="depth">The depth of the map.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the width or height are less than or equal to one.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the depth is equal to zero.</exception>
    public GridMapBase(uint width, uint height, uint depth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 1u, nameof(width));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(height, 1u, nameof(height));
        ArgumentOutOfRangeException.ThrowIfEqual(depth, 0u, nameof(depth));

        this.Width = width;
        this.Height = height;
        this.Depth = depth;

        this.cells = new Cell[width, height, depth];

        for (uint x = 0; x < width; x++)
        {
            for (uint y = 0; y < height; y++)
            {
                for (uint z = 0; z < depth; z++)
                {
                    this.cells[x, y, z] = new Cell(x, y, z, this);
                }
            }
        }
    }
}