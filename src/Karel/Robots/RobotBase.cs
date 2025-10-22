using Karel.Common;
using Karel.Scenarios.Maps;

namespace Karel.Robots;

/// <summary>
/// Base class for robots implementing common functionality.
/// </summary>
public abstract class RobotBase : ObservableBase, IRobot
{
    private Direction facing;
    private Cell position;

    /// <inheritdoc/>
    public Direction Facing
    {
        get => this.facing;
        protected set
        {
            if (!this.facing.Equals(value))
            {
                var oldValue = this.facing;
                this.facing = value;
                this.NotifyPropertyChanged(nameof(Facing), oldValue, value);
            }
        }
    }

    /// <inheritdoc/>
    public Cell Position
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

    /// <summary>
    /// Initializes a new instance of the <see cref="RobotBase"/> class.
    /// </summary>
    /// <param name="facing">The initial facing direction of the robot.</param>
    /// <param name="position">The initial position of the robot.</param>
    protected RobotBase(Direction facing, Cell position)
    {
        this.facing = facing;
        this.position = position;
    }

    /// <inheritdoc/>
    public void Move(MovementAxis axis)
    {
        // Implementation for moving the robot along the specified axis
        switch (axis)
        {
            case MovementAxis.X:
                // Move in the X direction based on the Facing direction
                break;
            case MovementAxis.Y:
                // Move in the Y direction based on the Facing direction
                break;
            case MovementAxis.Z:
                // Move in the Z direction based on the Facing direction
                break;
        }
    }
}
