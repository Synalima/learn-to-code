using Karel.Common;

namespace Karel.Scenarios.Maps;

/// <summary>
/// Represents a cell in a Karel scenario map.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Cell"/> class with specified coordinates and type.
/// </remarks>
public sealed class Cell(uint x, uint y, uint z, CellType type = CellType.Empty) : ObservableBase, ICell
{
    private CellType type = type;

    /// <inheritdoc/>
    public uint X { get; } = x;
    
    /// <inheritdoc/>
    public uint Y { get; } = y;

    /// <inheritdoc/>
    public uint Z { get; } = z;

    /// <inheritdoc/>
    public CellType Type
    {
        get => this.type;
        set
        {
            if (!this.type.Equals(value))
            {
                var oldValue = this.type;
                this.type = value;
                this.NotifyPropertyChanged(nameof(Type), oldValue, value);
            }
        }
    }
}