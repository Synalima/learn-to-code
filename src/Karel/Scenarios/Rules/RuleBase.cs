using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Karel.Robots;
using Karel.Scenarios.Maps;

namespace Karel.Scenarios.Rules;

/// <summary>
/// Base class for scenario rules implementing common functionality.
/// </summary>
public abstract class RuleBase : IRule
{
    protected IMap Map { get; private set; }
    protected ReadOnlyCollection<IRobot> Robots { get; private set; }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public virtual void Apply()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public virtual void Initialize()
    {
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public virtual bool IsApplicable()
    {
        return false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RuleBase"/> class with the specified map.
    /// </summary>
    /// <param name="map">The map associated with the rule.</param>
    /// <param name="robots">The collection of robots involved in the scenario.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="map"/> is null.</exception>
    public RuleBase(IMap map, ReadOnlyCollection<IRobot> robots)
    {
        ArgumentNullException.ThrowIfNull(map);
        ArgumentNullException.ThrowIfNull(robots);
        ArgumentOutOfRangeException.ThrowIfZero(robots.Count, nameof(robots));

        this.Map = map;
        this.Robots = robots;
    }
}