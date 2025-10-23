namespace Karel.Scenarios.Maps;

/// <summary>
/// A grid-based map for Karel the Robot.
/// </summary>
public abstract class GridMapBase : IMap
{
    private readonly Cell[,,] cells;

    /// <summary>
    /// The width of the map.
    /// </summary>
    public uint Width { get; }

    /// <summary>
    /// The height of the map.
    /// </summary>
    public uint Height { get; }

    /// <summary>
    /// The depth (number of layers) of the map.
    /// </summary>
    public uint Depth { get; }

    /// <summary>
    /// Checks if the specified coordinates are within the bounds of the map, including depth.
    /// </summary>
    /// <param name="x">The x-coordinate to check.</param>
    /// <param name="y">The y-coordinate to check.</param>
    /// <param name="z">The z-coordinate (layer) to check.</param>
    /// <returns>True if the coordinates are within bounds; otherwise, false.</returns>
    public bool InBounds(uint x, uint y, uint z) => x >= 0 && y >= 0 && z >= 0 && x < this.Width && y < this.Height && z < this.Depth;

    /// <summary>
    /// Gets the cell at the specified coordinates including depth (z).
    /// </summary>
    /// <param name="x">The x-coordinate of the cell.</param>
    /// <param name="y">The y-coordinate of the cell.</param>
    /// <param name="z">The z-coordinate (layer) of the cell.</param>
    /// <returns>The cell at the specified coordinates.</returns>
    public virtual Cell GetCell(uint x, uint y, uint z)
    {
        if (!InBounds(x, y, z))
        {
            throw new IndexOutOfRangeException("Cell out of bounds");
        }

        return this.cells[x, y, z];
    }

    /// <summary>
    /// Private constructor for internal use.
    /// </summary>
    private GridMapBase()
    {
        // Provide safe defaults so non-nullable fields/properties are initialized.
        this.cells = new Cell[this.Width, this.Height, this.Depth];
    }

    /// <summary>
    /// Creates a new GridMap with the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the map.</param>
    /// <param name="height">The height of the map.</param>
    /// <param name="depth">The depth of the map.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the width, height, or depth is less than or equal to zero.</exception>
    public GridMapBase(uint width, uint height, uint depth)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(width, 0u, nameof(width));
        ArgumentOutOfRangeException.ThrowIfEqual(height, 0u, nameof(height));
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
                    this.cells[x, y, z] = new Cell(x, y, z);
                }
            }
        }
    }
}