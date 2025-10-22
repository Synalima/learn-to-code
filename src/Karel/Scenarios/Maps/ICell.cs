namespace Karel.Scenarios.Maps;

/// <summary>
/// Types of cells in the map.
/// </summary>
[Flags]
public enum CellType
{
    /// <summary>
    /// An empty cell.
    /// </summary>
    Empty = 0,

    /// <summary>
    /// The starting position of the robot.
    /// </summary>
    StartPosition = 1,

    /// <summary>
    /// A cell that is obstructed.
    /// </summary>
    Obstructed = 2,
}

/// <summary>
/// Represents a cell in a Karel scenario map.
/// </summary>
public interface ICell
{
    /// <summary>
    /// The X coordinate of the cell.
    /// </summary>
    public uint X { get; }

    /// <summary>
    /// The Y coordinate of the cell.
    /// </summary>
    public uint Y { get; }

    /// <summary>
    /// The Z coordinate of the cell.
    /// </summary>
    public uint Z { get; }

    /// <summary>
    /// The type of the cell.
    /// </summary>
    public CellType Type { get; set; }
}
