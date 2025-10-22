using Karel.Common;

namespace Karel.Scenarios.Maps;

/// <summary>
/// Represents a cell in a Karel scenario map.
/// </summary>
public sealed class Cell : ObservableBase, ICell
{
    private uint x;
    private uint y;
    private uint z;

    private CellType type;

    /// <inheritdoc/>
    public uint X
    {
        get => x;
        set
        {
            if (x != value)
            {
                var oldValue = x;
                x = value;
                NotifyPropertyChanged(nameof(X), oldValue, value);
            }
        }
    }

    /// <inheritdoc/>
    public uint Y
    {
        get => y;
        set
        {
            if (y != value)
            {
                var oldValue = y;
                y = value;
                NotifyPropertyChanged(nameof(Y), oldValue, value);
            }
        }
    }

    /// <inheritdoc/>
    public uint Z
    {
        get => z;
        set
        {
            if (z != value)
            {
                var oldValue = z;
                z = value;
                NotifyPropertyChanged(nameof(Z), oldValue, value);
            }
        }
    }

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

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class with specified coordinates and type.
    /// </summary>
    public Cell(uint x, uint y, uint z, CellType type = CellType.Empty)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.type = type;
    }
}