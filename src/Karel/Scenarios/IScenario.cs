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
    /// Gets the map associated with the scenario.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the map is null.</exception>
    IMap Map { get; }

    /// <summary>
    /// Gets the read-only collection of robots participating in the scenario.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when there are no robots in the scenario.</exception>
    ReadOnlyObservableCollection<IRobot> Robots { get; }

    /// <summary>
    /// Runs the scenario.
    /// </summary>
    void Run();
}
