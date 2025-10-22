using Karel.Scenarios.Maps;

namespace Karel.Robots;

/// <summary>
/// Represents the possible directions a robot can face.
/// </summary>
public enum Direction { North, East, South, West }

/// <summary>
/// Represents the axes along which a robot can move.
/// </summary>
/// <remarks>
/// This enum is used to specify the axis of movement for the robot where X, Y, and Z correspond to the three-dimensional coordinate system. 
/// Moving along the X axis changes the robot's horizontal position (left and right), Y changes the vertical position (forward and backward), 
/// and Z would change depth (up and down) if applicable.
/// </remarks>
public enum MovementAxis { X, Y, Z }

/// <summary>
/// Defines the interface for a robot that can navigate a map, turn, and interact with its environment.
/// </summary>
public interface IRobot
{
    /// <summary>
    /// Gets the current position of the robot on the map.
    /// </summary>
    Cell Position { get; }

    /// <summary>
    /// Gets the direction the robot is currently facing.
    /// </summary>
    Direction Facing { get; }

    /// <summary>
    /// Moves the robot one unit forward in the direction indicated by the specified axis.
    /// </summary>
    /// <param name="axis">
    /// The axis along which to move the robot.
    /// </param>
    void Move(MovementAxis axis);
}
