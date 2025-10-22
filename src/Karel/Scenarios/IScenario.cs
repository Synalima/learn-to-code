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
    TimeSpan Duration { get; }

    /// <summary>
    /// Gets the map associated with the scenario.
    /// </summary>
    IMap Map { get; }

    /// <summary>
    /// Gets the read-only collection of rules applicable to the scenario.
    /// </summary>
    ReadOnlyCollection<IRule> Rules { get; }

    /// <summary>
    /// Gets the read-only collection of robots participating in the scenario.
    /// </summary>
    ReadOnlyCollection<IRobot> Robots { get; }

    /// <summary>
    /// Runs the scenario.
    /// </summary>
    void Run();
}
