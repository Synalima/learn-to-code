namespace Karel.Scenarios.Maps;

/// <summary>
/// A grid-based map that is flat (2D) for Karel the Robot.
/// </summary>
/// <param name="width">The width of the map.</param>
/// <param name="height">The height of the map.</param>
/// <remarks>
/// Creates a new FlatMap with the specified dimensions.
/// </remarks>
public class FlatMap : GridMapBase
{
    /// <summary>
    /// Creates a new <see cref="FlatMap"/> with the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the map.</param>
    /// <param name="height">The height of the map.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the width or height are less than or equal to one.</exception>
    /// <remarks>
    /// The map will have a depth of 1 and have a random StartPosition and Objective assigned.
    /// </remarks>
    public FlatMap(uint width, uint height) : base(width, height, 1) => this.AssignRandomStartAndObjective();
}