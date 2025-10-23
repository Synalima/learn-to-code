using System.Reflection;
using Xunit;

using Karel.Scenarios.Maps;
using Karel.Common;

namespace Karel.Tests.Scenarios.Maps
{
    /// BUG: Karel.Scenarios.Maps.FlatMap class does not have direct tests.
    /// This test class verifies the existence and basic structure of the FlatMap class, but is Copilot generated and not human useful/reviewed.
    public class FlatMapTests
    {
        private const string FlatMapFullName = "Karel.Scenarios.Maps.FlatMap";

        [Fact]
        public void FlatMap_HasConstructorWithTwoInt32s()
        {
            var map = new FlatMap(Random.Shared.Next(1u, 10u), Random.Shared.Next(11u, 20u));
            Assert.NotNull(map);
        }

        [Fact]
        public void FlatMap_CreatesInstanceWithProvidedWidth()
        {
            var expected = Random.Shared.Next(1u, 10u);
            var map = new FlatMap(expected, Random.Shared.Next(11u, 20u));
            Assert.Equal(expected, map.Width);
        }

        [Fact]
        public void FlatMap_CreatesInstanceWithProvidedHeight()
        {
            var expected = Random.Shared.Next(11u, 20u);
            var map = new FlatMap(Random.Shared.Next(1u, 10u), expected);
            Assert.Equal(expected, map.Height);
        }

        [Fact]
        public void FlatMap_HasDepthOfOne()
        {
            var map = new FlatMap(Random.Shared.Next(1u, 10u), Random.Shared.Next(11u, 20u));
            Assert.Equal(1u, map.Depth);
        }

        [Fact]
        public void FlatMap_InheritsFromGridMapBase()
        {
            var map = new FlatMap(Random.Shared.Next(1u, 10u), Random.Shared.Next(11u, 20u));
            Assert.IsAssignableFrom<GridMapBase>(map);
        }

        [Fact]
        public void FlatMap_GetCellThrowsArgumentOutOfRangeException_WithInvalidX()
        {
            var map = new FlatMap(Random.Shared.Next(1u, 10u), Random.Shared.Next(11u, 20u));
            Assert.Throws<ArgumentOutOfRangeException>(() => map.GetCell(map.Width, Random.Shared.Next(0u, map.Height - 1u), 1u));
        }

        [Fact]
        public void FlatMap_GetCellThrowsArgumentOutOfRangeException_WithInvalidY()
        {
            var map = new FlatMap(Random.Shared.Next(1u, 10u), Random.Shared.Next(11u, 20u));
            Assert.Throws<ArgumentOutOfRangeException>(() => map.GetCell(Random.Shared.Next(0u, map.Width - 1u), map.Height, 1u));
        }

        [Fact]
        public void FlatMap_GetCellThrowsArgumentOutOfRangeException_WithInvalidZ()
        {
            var map = new FlatMap(Random.Shared.Next(1u, 10u), Random.Shared.Next(11u, 20u));
            Assert.Throws<ArgumentOutOfRangeException>(() => map.GetCell(Random.Shared.Next(0u, map.Width - 1u), Random.Shared.Next(0u, map.Height - 1u), Random.Shared.Next(1u, 10u)));
        }

        [Fact]
        public void FlatMap_ConstructorThrowsArgumentOutOfRangeException_WithInvalidWidth()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FlatMap(0u, Random.Shared.Next(11u, 20u)));
        }

        [Fact]
        public void FlatMap_ConstructorThrowsArgumentOutOfRangeException_WithInvalidHeight()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FlatMap(Random.Shared.Next(1u, 10u), 0u));
        }

        [Fact]
        public void FlatMap_ConstructorInitializesCells()
        {
            var map = new FlatMap(Random.Shared.Next(1u, 10u), Random.Shared.Next(11u, 20u));
            Assert.NotNull(map.GetCell(0, 0, 0));
        }

        [Fact]
        public void FlatMap_GetCellReturnsCorrectCell()
        {
            var width = Random.Shared.Next(1u, 10u);
            var height = Random.Shared.Next(11u, 20u);
            var map = new FlatMap(width, height);

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    var cell = map.GetCell(x, y, 0);
                    Assert.Equal(x, cell.X);
                    Assert.Equal(y, cell.Y);
                    Assert.Equal(0u, cell.Z);
                }
            }
        }

        [Fact]
        public void FlatMap_InboundsReturnsTrueForValidCoordinates()
        {
            var width = Random.Shared.Next(1u, 10u);
            var height = Random.Shared.Next(11u, 20u);
            var map = new FlatMap(width, height);

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    Assert.True(map.InBounds(x, y, 0));
                }
            }
         }
    }
}