namespace Karel.Scenarios.Maps;

/// <summary>
/// Extension helpers for <see cref="GridMapBase"/>.
/// </summary>
public static class GridMapBaseExtensions
{
	/// <summary>
	/// Randomly assigns exactly one StartPosition and one Objective cell on a flat map (depth=1).
	/// Both designated cells will also have the <see cref="CellType.Empty"/> flag set.
	/// </summary>
	/// <param name="map">The map to operate on.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="map"/> is null.</exception>
	public static void AssignRandomStartAndObjective(this GridMapBase map)
	{
        ArgumentNullException.ThrowIfNull(map);

        var total = map.Width * map.Height;

		var first = Random.Shared.NextInt64(0, total);
		long second;
		do
		{
			second = Random.Shared.NextInt64(0, total);
		} while (second == first);

		uint fx = (uint)(first % map.Width);
		uint fy = (uint)(first / map.Width);

		uint sx = (uint)(second % map.Width);
		uint sy = (uint)(second / map.Width);

		if (map.TryGetCell(fx, fy, 0u, out var fcell))
		{
			fcell!.Type = CellType.StartPosition | CellType.Empty;
		}

		if (map.TryGetCell(sx, sy, 0u, out var scell))
		{
			scell!.Type = CellType.Objective | CellType.Empty;
		}
	}
}
