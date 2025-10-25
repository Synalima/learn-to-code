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
    public ReadOnlyObservableCollection<IRobot> Robots { get; init; }

    /// <inheritdoc/>
    public virtual void Run(int delay = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(delay, 0, nameof(delay));

        bool rulesApplicable = this.Rules.Any(rule => rule.IsApplicable(this.Map, this.Robots));
        while (rulesApplicable && DateTime.Now.TimeOfDay - this.StartTime < this.Duration)
        {
            this.Rules.AsParallel().ForAll(rule => rule.Apply(this.Map, this.Robots));
            this.Robots.AsParallel().ForAll(robot => robot.Act());
            rulesApplicable = this.Rules.Any(rule => rule.IsApplicable(this.Map, this.Robots));
            Task.Delay(delay).Wait();
        }
    }

    /// <summary>
    /// Protected constructor to initialize the scenario with specified parameters.
    /// </summary>
    /// <param name="duration">The duration of the scenario.</param>
    /// <param name="map">The map associated with the scenario.</param>
    /// <param name="rules">The rules applicable to the scenario.</param>
    /// <param name="robots">The robots involved in the scenario.</param>
    protected ScenarioBase(TimeSpan duration, IMap map, ReadOnlyCollection<IRule> rules, ReadOnlyObservableCollection<IRobot> robots)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        ArgumentNullException.ThrowIfNull(robots, nameof(robots));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(duration, TimeSpan.Zero, nameof(duration));
        ArgumentOutOfRangeException.ThrowIfZero(robots.Count, nameof(robots));
        
        this.Duration = duration;
        this.Map = map;
        this.Rules = rules;
        this.Robots = robots;
    }
}
