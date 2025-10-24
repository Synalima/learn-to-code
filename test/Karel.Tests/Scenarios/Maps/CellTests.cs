using Karel.Common;
using Karel.Scenarios.Maps;
using Xunit;

namespace Karel.Tests.Scenarios.Maps;

/// <summary>
/// Tests for the Cell class.
/// </summary>
public sealed class CellTests
{
    [Fact]
    public void Constructor_ShouldInitializeCoordinates()
    {
        var cell = new Cell(5, 10, 15);

        Assert.Equal(5u, cell.X);
        Assert.Equal(10u, cell.Y);
        Assert.Equal(15u, cell.Z);
    }

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyTypeByDefault()
    {
        var cell = new Cell(1, 2, 3);

        Assert.Equal(CellType.Empty, cell.Type);
    }

    [Fact]
    public void Constructor_ShouldInitializeWithSpecifiedType()
    {
        var cell = new Cell(1, 2, 3, CellType.StartPosition);

        Assert.Equal(CellType.StartPosition, cell.Type);
    }

    [Fact]
    public void Type_ShouldUpdateValue()
    {
        var cell = new Cell(1, 2, 3, CellType.Empty);

        cell.Type = CellType.Objective;

        Assert.Equal(CellType.Objective, cell.Type);
    }

    [Fact]
    public void Type_ShouldNotifyPropertyChanged_WhenValueChanges()
    {
        var cell = new Cell(1, 2, 3, CellType.Obstructed);
        var propertyChangedRaised = false;
        string changedPropertyName = string.Empty;
        CellType oldValue = cell.Type;
        CellType newValue = CellType.Empty;

        cell.PropertyChanged += (sender, args) =>
        {
            propertyChangedRaised = true;
            changedPropertyName = args.PropertyName ?? string.Empty;

            var nargs = (PropertyChangedEventArgs<CellType>)args;
            oldValue = nargs.OldValue;
            newValue = nargs.NewValue;
        };

        cell.Type = CellType.Objective;

        Assert.True(propertyChangedRaised);
        Assert.Equal(nameof(Cell.Type), changedPropertyName);
        Assert.Equal(CellType.Obstructed, oldValue);
        Assert.Equal(CellType.Objective, newValue);
    }

    [Fact]
    public void Type_ShouldNotNotifyPropertyChanged_WhenValueIsTheSame()
    {
        var cell = new Cell(1, 2, 3, CellType.Empty);
        var propertyChangedRaised = false;

        cell.PropertyChanged += (sender, args) =>
        {
            propertyChangedRaised = true;
        };

        cell.Type = CellType.Empty;

        Assert.False(propertyChangedRaised);
    }

    [Fact]
    public void Coordinates_ShouldBeReadOnly()
    {
        var cell = new Cell(5, 10, 15);

        Assert.Equal(5u, cell.X);
        Assert.Equal(10u, cell.Y);
        Assert.Equal(15u, cell.Z);
    }
}