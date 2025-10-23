namespace Karel.Scenarios.Maps;

/// <summary>
/// A grid-based map that is flat (2D) for Karel the Robot.
/// </summary>
public class FlatMap : GridMapBase
{
    /// <summary>
    /// Creates a new FlatMap with the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the map.</param>
    /// <param name="height">The height of the map.</param>
    public FlatMap(uint width, uint height) : base(width, height, 1)
    {
    }

    /// <inheritdoc/>
    public override Cell GetCell(uint x, uint y, uint z = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(z, 0u, nameof(z));
        
        return base.GetCell(x, y, z);
    }
}