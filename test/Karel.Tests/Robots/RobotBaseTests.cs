using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

using Karel.Robots;
using Karel.Scenarios.Maps;

namespace Karel.Tests.Robots
{
    public class RobotBaseTests
    {
        private class FakeCell : ICell
        {
            public FakeCell(uint x, uint y, uint z, CellType type = CellType.Empty)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
                this.Type = type;
            }

            public uint X { get; set; }
            public uint Y { get; set; }
            public uint Z { get; set; }
            public CellType Type { get; set; }
        }

        private class FakeMap : IMap
        {
            private readonly Dictionary<(uint, uint, uint), ICell> cells = new();

            public uint Width { get; private set; }
            public uint Height { get; private set; }
            public uint Depth { get; private set; }

            public FakeMap(uint width, uint height, uint depth)
            {
                this.Width = width;
                this.Height = height;
                this.Depth = depth;
            }

            public void AddCell(ICell cell)
            {
                if (cell is Cell concrete)
                {
                    this.cells[(cell.X, cell.Y, cell.Z)] = concrete;
                }
                else
                {
                    this.cells[(cell.X, cell.Y, cell.Z)] = new Cell(cell.X, cell.Y, cell.Z, cell.Type);
                }
            }

            public bool InBounds(uint x, uint y, uint z) => x < this.Width && y < this.Height && z < this.Depth;

            public bool TryGetCell(uint x, uint y, uint z, out Cell? cell)
            {
                if (this.cells.TryGetValue((x, y, z), out var c) && c is Cell concrete)
                {
                    cell = concrete;
                    return true;
                }

                cell = null;
                return false;
            }

            public IEnumerable<ICell> GetOrthogonalNeighbors(ICell cell) => GetOrthogonalNeighbors(cell.X, cell.Y, cell.Z);

            public IEnumerable<ICell> GetOrthogonalNeighbors(uint x, uint y, uint z)
            {
                var deltas = new (int dx, int dy, int dz)[] { (1,0,0), (-1,0,0), (0,1,0), (0,-1,0),(0,0,1),(0,0,-1) };
                foreach (var (dx,dy,dz) in deltas)
                {
                    long nx = (long)x + dx; long ny = (long)y + dy; long nz = (long)z + dz;
                    if (nx >= 0 && ny >= 0 && nz >= 0 && (uint)nx < this.Width && (uint)ny < this.Height && (uint)nz < this.Depth)
                    {
                        if (this.cells.TryGetValue(((uint)nx,(uint)ny,(uint)nz), out var c)) yield return c;
                    }
                }
            }

            public IEnumerable<ICell> GetAllAdjacentNeighbors(ICell cell) => GetAllAdjacentNeighbors(cell.X, cell.Y, cell.Z);

            public IEnumerable<ICell> GetAllAdjacentNeighbors(uint x, uint y, uint z)
            {
                for (int dz = -1; dz <= 1; dz++)
                    for (int dy = -1; dy <= 1; dy++)
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (dx == 0 && dy == 0 && dz == 0) continue;
                            long nx = (long)x + dx; long ny = (long)y + dy; long nz = (long)z + dz;
                            if (nx >= 0 && ny >= 0 && nz >= 0 && (uint)nx < this.Width && (uint)ny < this.Height && (uint)nz < this.Depth)
                            {
                                if (this.cells.TryGetValue(((uint)nx,(uint)ny,(uint)nz), out var c)) yield return c;
                            }
                        }
            }
        }

        private class ConcreteRobot : RobotBase
        {
            public ConcreteRobot(ICell position, IMap map) : base(position, map) { }
            public override void Act() { }
        }

        [Fact]
        public void TryMoveTo_TargetCellDoesNotExist_ReturnsFalseAndError()
        {
            var map = new FakeMap(3,3,1);
            var start = new FakeCell(0,0,0);
            map.AddCell(start);
            var robot = new ConcreteRobot(start, map);

            var ok = robot.TryMoveTo(2,2, out var errors);

            Assert.False(ok);
            Assert.Contains(errors, e => e.Contains("does not exist", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void TryMoveTo_TargetCellNotNavigable_ReturnsFalseAndError()
        {
            var map = new FakeMap(3,3,1);
            var start = new FakeCell(0,0,0);
            var target = new FakeCell(1,0,0, CellType.Obstructed);
            map.AddCell(start);
            map.AddCell(target);
            var robot = new ConcreteRobot(start, map);

            var ok = robot.TryMoveTo(1,0, out var errors);

            Assert.False(ok);
            Assert.Contains(errors, e => e.Contains("not navigable", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void TryMoveTo_TargetNotAdjacent_ReturnsFalseAndError()
        {
            var map = new FakeMap(4,4,1);
            var start = new FakeCell(0,0,0);
            var target = new FakeCell(2,2,0, CellType.Empty);
            map.AddCell(start);
            map.AddCell(target);
            var robot = new ConcreteRobot(start, map);

            var ok = robot.TryMoveTo(2,2, out var errors);

            Assert.False(ok);
            Assert.Contains(errors, e => e.Contains("not adjacent", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void TryMoveTo_Succeeds_WhenTargetIsAdjacentAndNavigable()
        {
            var map = new FakeMap(3,3,1);
            var start = new FakeCell(1,1,0);
            var target = new FakeCell(2,1,0, CellType.Empty);
            map.AddCell(start);
            map.AddCell(target);
            var robot = new ConcreteRobot(start, map);

            var ok = robot.TryMoveTo(2,1, out var errors);

            Assert.True(ok);
            Assert.Empty(errors);
            Assert.Equal(target.X, robot.Position.X);
            Assert.Equal(target.Y, robot.Position.Y);
            Assert.Equal(target.Z, robot.Position.Z);
        }

        [Fact]
        public void Position_Set_RaisesPropertyChanged_WithOldAndNewValues()
        {
            var map = new FakeMap(3,3,1);
            var start = new FakeCell(1,1,0);
            var target = new FakeCell(2,1,0, CellType.Empty);
            map.AddCell(start);
            map.AddCell(target);
            var robot = new ConcreteRobot(start, map);

            bool raised = false;
            Karel.Common.PropertyChangedEventArgs<ICell>? received = null;
            robot.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(robot.Position) && args is Karel.Common.PropertyChangedEventArgs<ICell> pa)
                {
                    raised = true;
                    received = pa;
                }
            };

            robot.Position = target;

            Assert.True(raised);
            Assert.NotNull(received);
            Assert.Equal(start, received!.OldValue);
            Assert.Equal(target, received.NewValue);
        }

        private class FakeMapThrows : IMap
        {
            public uint Width => 1;
            public uint Height => 1;
            public uint Depth => 1;

            public bool InBounds(uint x, uint y, uint z) => true;

            public bool TryGetCell(uint x, uint y, uint z, out Cell? cell)
            {
                throw new InvalidOperationException("boom");
            }

            public IEnumerable<ICell> GetOrthogonalNeighbors(ICell cell) => Enumerable.Empty<ICell>();
            public IEnumerable<ICell> GetOrthogonalNeighbors(uint x, uint y, uint z) => Enumerable.Empty<ICell>();
            public IEnumerable<ICell> GetAllAdjacentNeighbors(ICell cell) => Enumerable.Empty<ICell>();
            public IEnumerable<ICell> GetAllAdjacentNeighbors(uint x, uint y, uint z) => Enumerable.Empty<ICell>();
        }

        [Fact]
        public void TryMoveTo_WhenMapThrows_ReturnsFalseAndContainsExceptionMessage()
        {
            var map = new FakeMapThrows();
            var start = new FakeCell(0,0,0);
            var robot = new ConcreteRobot(start, map);

            var ok = robot.TryMoveTo(1,1, out var errors);

            Assert.False(ok);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.IndexOf("boom", StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
