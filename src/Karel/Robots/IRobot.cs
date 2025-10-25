using Karel.Scenarios.Maps;

namespace Karel.Robots;

/// <summary>
/// Defines the interface for a robot that can navigate a map, turn, and interact with its environment.
/// </summary>
public interface IRobot
{
    /// <summary>
    /// Gets the current position of the robot on the map.
    /// </summary>
    ICell Position { get; }

    /// <summary>
    /// Performs an action defined by the robot.
    /// </summary>
    void Act();

    /// <summary>
    /// Attempts to move the robot to the specified cell.
    /// </summary>
    /// <param name="x">The x-coordinate of the target cell.</param>
    /// <param name="y">The y-coordinate of the target cell.</param>
    /// <param name="errors">A list of errors that occurred during the move attempt.</param>
    /// <returns>True if the move was successful; otherwise, false.</returns>
    bool TryMoveTo(uint x, uint y, out IList<string> errors);

    /// <summary>
    /// Attempts to move the robot to the specified cell.
    /// </summary>
    /// <param name="x">The x-coordinate of the target cell.</param>
    /// <param name="y">The y-coordinate of the target cell.</param>
    /// <param name="z">The z-coordinate of the target cell.</param>
    /// <param name="errors">A list of errors that occurred during the move attempt.</param>
    /// <returns>True if the move was successful; otherwise, false.</returns>
    bool TryMoveTo(uint x, uint y, uint z, out IList<string> errors);
}
