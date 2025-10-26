using Xunit;
using Karel.Scenarios.Maps;

namespace Karel.Tests.Scenarios.Maps
{
    /// <summary>
    /// Tests for Cell extension methods.
    /// </summary>
    public class CellExtensionsTests
    {
        [Fact]
        public void IsEmpty_ReturnsTrue_WhenCellHasEmptyFlag()
        {
            var cell = new FakeCell(CellType.Empty);
            Assert.True(cell.IsEmpty());
        }

        [Fact]
        public void IsEmpty_ReturnsFalse_WhenCellDoesNotHaveEmptyFlag()
        {
            var cell = new FakeCell(CellType.Obstructed);
            Assert.False(cell.IsEmpty());
        }

        [Fact]
        public void IsObstructed_ReturnsTrue_WhenCellHasObstructedFlag()
        {
            var cell = new FakeCell(CellType.Obstructed);
            Assert.True(cell.IsObstructed());
        }

        [Fact]
        public void IsObstructed_ReturnsFalse_WhenCellDoesNotHaveObstructedFlag()
        {
            var cell = new FakeCell(CellType.Empty);
            Assert.False(cell.IsObstructed());
        }

        [Fact]
        public void IsObjective_ReturnsTrue_WhenCellHasObjectiveFlag()
        {
            var cell = new FakeCell(CellType.Objective);
            Assert.True(cell.IsObjective());
        }

        [Fact]
        public void IsObjective_ReturnsFalse_WhenCellDoesNotHaveObjectiveFlag()
        {
            var cell = new FakeCell(CellType.Empty);
            Assert.False(cell.IsObjective());
        }

        [Fact]
        public void IsStartPosition_ReturnsTrue_WhenCellHasStartPositionFlag()
        {
            var cell = new FakeCell(CellType.StartPosition);
            Assert.True(cell.IsStartPosition());
        }

        [Fact]
        public void IsStartPosition_ReturnsFalse_WhenCellDoesNotHaveStartPositionFlag()
        {
            var cell = new FakeCell(CellType.Empty);
            Assert.False(cell.IsStartPosition());
        }

        [Fact]
        public void CanNavigate_ReturnsTrue_WhenCellIsNotObstructed()
        {
            var cell = new FakeCell(CellType.Empty | CellType.Objective | CellType.StartPosition);
            Assert.True(cell.CanNavigate());
        }

        [Fact]
        public void CanNavigate_ReturnsFalse_WhenCellIsObstructed()
        {
            var cell = new FakeCell(CellType.Obstructed);
            Assert.False(cell.CanNavigate());
        }

        [Fact]
        public void CombinedFlags_WorkAsExpected()
        {
            var nonObstructed = new FakeCell(CellType.Empty | CellType.Objective);
            Assert.True(nonObstructed.IsEmpty());
            Assert.True(nonObstructed.IsObjective());
            Assert.False(nonObstructed.IsObstructed());
            Assert.True(nonObstructed.CanNavigate());

            var obstructedObjective = new FakeCell(CellType.Obstructed | CellType.Objective);
            Assert.True(obstructedObjective.IsObjective());
            Assert.True(obstructedObjective.IsObstructed());
            Assert.False(obstructedObjective.IsEmpty());
            Assert.False(obstructedObjective.CanNavigate());
        }

        private sealed class FakeCell(CellType type) : ICell
        {
            public CellType Type { get; set; } = type;

            public uint X => 1u;

            public uint Y => 2u;

            public uint Z => 3u;

            public IMap Map => throw new NotImplementedException();
        }
    }
}