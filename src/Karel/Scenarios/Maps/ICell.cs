namespace Karel.Scenarios.Maps;

/// <summary>
/// Types of cells in the map.
/// </summary>
[Flags]
public enum CellType
{
    /// <summary>
    /// No special type.
    /// </summary>
    None = 0,

    /// <summary>
    /// An empty cell.
    /// </summary>
    Empty = 1,

    /// <summary>
    /// A starting position of the robot in the scenario.
    /// </summary>
    StartPosition = 2,

    /// <summary>
    /// A cell that is obstructed, it can be neither entered nor passed through.
    /// </summary>
    Obstructed = 4,

    /// <summary>
    /// An objective cell.    
    /// </summary>
    Objective = 8
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

    /// <summary>
    /// The map this cell belongs to.
    /// </summary>
    public IMap Map { get; }
}
