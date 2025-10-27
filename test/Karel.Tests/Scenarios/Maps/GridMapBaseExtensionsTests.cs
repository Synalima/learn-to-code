using Xunit;
using Karel.Scenarios.Maps;

namespace Karel.Tests.Scenarios.Maps;

public class GridMapBaseExtensionsTests
{
	[Fact]
	public void AssignRandomStartAndObjective_SetsExactlyOneStartAndOneObjective_AllCellsHaveEmptyFlag()
	{
		// Arrange: create a concrete GridMapBase subclass that doesn't auto-assign in ctor.
		var width = 5u;
		var height = 5u;
		var map = new TestMap(width, height);

		// Act
		GridMapBaseExtensions.AssignRandomStartAndObjective(map);

		// Assert
		int startCount = 0, objectiveCount = 0, emptyCount = 0, total = 0;

		for (uint x = 0; x < width; x++)
		{
			for (uint y = 0; y < height; y++)
			{
				total++;
				var ok = map.TryGetCell(x, y, 0u, out var cell);
				Assert.True(ok);
				Assert.NotNull(cell);

				var type = cell!.Type;
				if (type.HasFlag(CellType.StartPosition)) startCount++;
				if (type.HasFlag(CellType.Objective)) objectiveCount++;
				if (type.HasFlag(CellType.Empty)) emptyCount++;
			}
		}

		Assert.Equal(1, startCount);
		Assert.Equal(1, objectiveCount);
		Assert.Equal(total, emptyCount);
	}

	// Small concrete map that does not perform any automatic assignment in ctor.
	private sealed class TestMap : GridMapBase
	{
		public TestMap(uint width, uint height) : base(width, height, 1) { }
	}

	[Fact]
	public void AssignRandomStartAndObjective_Throws_IfMapIsNull()
	{
		Assert.Throws<ArgumentNullException>(() => GridMapBaseExtensions.AssignRandomStartAndObjective(null!));
	}
}
