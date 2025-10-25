using Karel.Common;
using Karel.Scenarios.Maps;

namespace Karel.Robots;

/// <summary>
/// Base class for robots implementing common functionality.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RobotBase"/> class.
/// </remarks>
/// <param name="position">The initial position of the robot.</param>
public abstract class RobotBase(ICell position, IMap map) : ObservableBase, IRobot
{
    private ICell position = position;
    private readonly IMap map = map;

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
            if (!this.map.TryGetCell(x, y, z, out var targetCell))
            {
                errors.Add("Target cell does not exist.");
                return false;
            }

            if (!targetCell!.CanNavigate())
            {
                errors.Add("Target cell is not navigable.");
                return false;
            }

            if (!this.map.GetAllAdjacentNeighbors(this.Position).Contains(targetCell))
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
