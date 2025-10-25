namespace Karel.Scenarios.Maps;

/// <summary>
/// Interface for a 3D grid-based map for Karel the Robot.
/// </summary>
public interface IMap
{
    /// <summary>
    /// The width of the map.
    /// </summary>
    uint Width { get; }

    /// <summary>
    /// The height of the map.
    /// </summary>
    uint Height { get; }

    /// <summary>
    /// The depth of the map.
    /// </summary>
    uint Depth { get; }

    /// <summary>
    /// Checks if the specified coordinates are within the bounds of the map.
    /// </summary>
    /// <param name="x">The x-coordinate to check.</param>
    /// <param name="y">The y-coordinate to check.</param>
    /// <param name="z">The z-coordinate to check.</param>
    /// <returns>True if the coordinates are within bounds; otherwise, false.</returns>
    bool InBounds(uint x, uint y, uint z);

    /// <summary>
    /// Gets the cell at the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the cell.</param>
    /// <param name="y">The y-coordinate of the cell.</param>
    /// <param name="z">The z-coordinate of the cell.</param>
    /// <param name="cell">The cell at the specified coordinates.</param>
    /// <returns>The cell at the specified coordinates.</returns>
    bool TryGetCell(uint x, uint y, uint z, out Cell? cell);

    /// <summary>
    /// Gets the orthogonal neighbors of the specified cell.
    /// </summary>
    /// <param name="cell">The cell whose neighbors to retrieve.</param>
    /// <returns>An enumerable collection of orthogonal neighbor cells.</returns>
    IEnumerable<ICell> GetOrthogonalNeighbors(ICell cell);

    /// <summary>
    /// Gets the orthogonal neighbors of the cell at the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the cell.</param>
    /// <param name="y">The y-coordinate of the cell.</param>
    /// <param name="z">The z-coordinate of the cell.</param>
    /// <returns>An enumerable collection of orthogonal neighbor cells.</returns>
    IEnumerable<ICell> GetOrthogonalNeighbors(uint x, uint y, uint z);

    /// <summary>
    /// Returns all adjacent neighbors in the 3x3x3 cube around the specified cell (up to 26 neighbors).
    /// The center cell is excluded. Out-of-bounds neighbors are omitted.
    /// </summary>
    /// <param name="cell">The cell whose neighbors to retrieve.</param>
    /// <returns>An enumerable collection of adjacent neighbor cells.</returns>
    IEnumerable<ICell> GetAllAdjacentNeighbors(ICell cell);

    /// <summary>
    /// Returns all adjacent neighbors in the 3x3x3 cube around the specified cell (up to 26 neighbors).
    /// The center cell is excluded. Out-of-bounds neighbors are omitted.
    /// </summary>
    /// <param name="x">The x-coordinate of the cell.</param>
    /// <param name="y">The y-coordinate of the cell.</param>
    /// <param name="z">The z-coordinate of the cell.</param>
    /// <returns>An enumerable collection of adjacent neighbor cells.</returns>
    IEnumerable<ICell> GetAllAdjacentNeighbors(uint x, uint y, uint z);
}
