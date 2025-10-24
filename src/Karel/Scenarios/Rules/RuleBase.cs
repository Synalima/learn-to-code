using Karel.Robots;
using Karel.Scenarios.Maps;

namespace Karel.Scenarios.Rules;

/// <summary>
/// Base class for scenario rules implementing common functionality.
/// </summary>
public abstract class RuleBase : IRule
{
    /// <inheritdoc/>
    public virtual bool IsApplicable(IMap map, IReadOnlyCollection<IRobot> robots)
    {
        return false;
    }

    /// <inheritdoc/>
    public virtual void Apply(IMap map, IReadOnlyCollection<IRobot> robots)
    {
        if(!this.IsApplicable(map, robots))
        {
            return;
        }
    }
}