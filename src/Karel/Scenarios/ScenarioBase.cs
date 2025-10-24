using System.Collections.ObjectModel;
using Karel.Robots;
using Karel.Scenarios.Maps;
using Karel.Scenarios.Rules;

namespace Karel.Scenarios;

/// <summary>
/// Base class for Karel scenarios implementing common functionality.
/// </summary>
public abstract class ScenarioBase : IScenario
{
    private TimeSpan StartTime { get; } = DateTime.Now.TimeOfDay;

    /// <inheritdoc/>
    public TimeSpan Duration { get; }

    /// <inheritdoc/>
    public IMap Map { get; }

    /// <inheritdoc/>
    public ReadOnlyCollection<IRule> Rules { get; }

    /// <inheritdoc/>
    public ReadOnlyCollection<IRobot> Robots { get; init; }

    /// <inheritdoc/>
    public virtual void Run()
    {
        while (DateTime.Now.TimeOfDay - this.StartTime < this.Duration)
        {
            this.Rules.AsParallel().ForAll(rule => rule.Apply(this.Map, this.Robots));
        }
    }

    /// <summary>
    /// Protected constructor to initialize the scenario with specified parameters.
    /// </summary>
    /// <param name="duration">The duration of the scenario.</param>
    /// <param name="map">The map associated with the scenario.</param>
    /// <param name="rules">The rules applicable to the scenario.</param>
    /// <param name="robots">The robots involved in the scenario.</param>
    protected ScenarioBase(TimeSpan duration, IMap map, ReadOnlyCollection<IRule> rules, ReadOnlyCollection<IRobot> robots)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(duration, TimeSpan.Zero, nameof(duration));

        this.Duration = duration;
        this.Map = map ?? throw new ArgumentNullException(nameof(map));
        this.Rules = rules ?? throw new ArgumentNullException(nameof(rules));
        this.Robots = robots ?? throw new ArgumentNullException(nameof(robots));
    }
}
