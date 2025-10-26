using Karel.Common;
using Karel.Scenarios.Maps;

namespace Karel.Robots;

/// <summary>
/// Base class for robots implementing common functionality.
/// </summary>
public abstract class RobotBase() : ObservableBase, IRobot
{
    private ICell position = null!;

    /// <inheritdoc/>
    public ICell Position
    {
        get => this.position;
        set
        {
            if (!Equals(this.position, value))
            {
                var oldValue = this.position;
                this.position = value;
                this.NotifyPropertyChanged(nameof(Position), oldValue, value);
            }
        }
    }

    /// <inheritdoc/>
    public abstract void Act();

    /// <inheritdoc/>
    public void Initialize(ICell initialPosition)
    {
        ArgumentNullException.ThrowIfNull(initialPosition);
        ArgumentOutOfRangeException.ThrowIfEqual(initialPosition.Map.InBounds(initialPosition), false, nameof(initialPosition));

        this.Position = initialPosition;
    }

    /// <inheritdoc/>
    public bool TryMoveTo(uint x, uint y, out IList<string> errors)
    {
        return this.TryMoveTo(x, y, 0, out errors);
    }

    /// <inheritdoc/>
    public bool TryMoveTo(uint x, uint y, uint z, out IList<string> errors)
    {
        errors = [];

        try
        {
            if (!this.Position.Map.TryGetCell(x, y, z, out var targetCell))
            {
                errors.Add("Target cell does not exist.");
                return false;
            }

            if (!targetCell!.CanNavigate())
            {
                errors.Add("Target cell is not navigable.");
                return false;
            }

            if (!this.Position.Map.GetAllAdjacentNeighbors(this.Position).Contains(targetCell))
            {
                errors.Add("Target cell is not adjacent.");
                return false;
            }

            this.Position = targetCell!;
            return true;
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
            return false;
        }
    }
}
