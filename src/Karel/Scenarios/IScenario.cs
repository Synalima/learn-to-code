using System.Collections.ObjectModel;
using Karel.Robots;
using Karel.Scenarios.Maps;
using Karel.Scenarios.Rules;

namespace Karel.Scenarios;

/// <summary>
/// Defines a scenario in which Karel operates.
/// </summary>
public interface IScenario
{
    /// <summary>
    /// Gets the total duration of the scenario.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the duration is less than or equal to zero.</exception>
    TimeSpan Duration { get; }

    /// <summary>
    /// Gets the map associated with the scenario.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the map is null.</exception>
    IMap Map { get; }

    /// <summary>
    /// Gets the read-only collection of rules applicable to the scenario.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when there are no rules in the scenario.</exception>
    ReadOnlyCollection<IRule> Rules { get; }

    /// <summary>
    /// Gets the read-only collection of robots participating in the scenario.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when there are no robots in the scenario.</exception>
    ReadOnlyObservableCollection<IRobot> Robots { get; }

    /// <summary>
    /// Runs the scenario.
    /// </summary>
    /// <param name="delay">The delay in milliseconds between each action cycle. Recommended values are 1-5 ms for unit/integration tests.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="delay"/> is less than or equal to zero.</exception>
    void Run(int delay = 1);
}
