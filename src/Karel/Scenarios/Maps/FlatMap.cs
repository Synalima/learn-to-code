namespace Karel.Scenarios.Maps;

/// <summary>
/// A grid-based map that is flat (2D) for Karel the Robot.
/// </summary>
/// <param name="width">The width of the map.</param>
/// <param name="height">The height of the map.</param>
/// <remarks>
/// Creates a new FlatMap with the specified dimensions.
/// </remarks>
public class FlatMap(uint width, uint height) : GridMapBase(width, height, 1)
{
}