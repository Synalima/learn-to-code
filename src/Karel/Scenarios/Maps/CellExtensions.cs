namespace Karel.Scenarios.Maps;

/// <summary>
/// Extension methods for the <see cref="ICell"/> interface.
/// </summary>
public static class CellExtensions
{
    /// <summary>
    /// Determines whether the cell is empty.
    /// </summary>
    /// <param name="cell">The cell to check.</param>
    /// <returns><c>true</c> if the cell is empty; otherwise, <c>false</c>.</returns>
    public static bool IsEmpty(this ICell cell)
    {
        return (cell.Type & CellType.Empty) == CellType.Empty;
    }

    /// <summary>
    /// Determines whether the cell can be navigated.
    /// </summary>
    /// <param name="cell">The cell to check.</param>
    /// <returns><c>true</c> if the cell can be navigated; otherwise, <c>false</c>.</returns>
    public static bool CanNavigate(this ICell cell)
    {
        return !cell.IsObstructed();
    }
    
    /// <summary>
    /// Determines whether the cell is obstructed.
    /// </summary>
    /// <param name="cell">The cell to check.</param>
    /// <returns><c>true</c> if the cell is obstructed; otherwise, <c>false</c>.</returns>
    public static bool IsObstructed(this ICell cell)
    {
        return (cell.Type & CellType.Obstructed) == CellType.Obstructed;
    }

    /// <summary>
    /// Determines whether the cell is an objective.
    /// </summary>
    /// <param name="cell">The cell to check.</param>
    /// <returns><c>true</c> if the cell is an objective; otherwise, <c>false</c>.</returns>
    public static bool IsObjective(this ICell cell)
    {
        return (cell.Type & CellType.Objective) == CellType.Objective;
    }

    /// <summary>
    /// Determines whether the cell is a starting position.
    /// </summary>
    /// <param name="cell">The cell to check.</param>
    /// <returns><c>true</c> if the cell is a starting position; otherwise, <c>false</c>.</returns>
    public static bool IsStartPosition(this ICell cell)
    {
        return (cell.Type & CellType.StartPosition) == CellType.StartPosition;
    }
}
