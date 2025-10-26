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
    /// <inheritdoc/>
    public IMap Map { get; }

    /// <inheritdoc/>
    public ReadOnlyCollection<IRule> Rules { get; }

    /// <inheritdoc/>
    public ReadOnlyObservableCollection<IRobot> Robots { get; init; }

    /// <inheritdoc/>
    /// <summary>
    /// Executes the scenario by running robot actions and applying rules until no further rules are applicable.
    /// </summary>
    /// <remarks>
    /// The scenario runs in a loop where each robot performs its action, followed by the application of all applicable rules.
    /// </remarks>
    public void Run()
    {
        // TODO: Initialize robots' positions before starting the scenario, prefer CellType.Start if available.

        bool rulesApplicable;
        do
        {
            rulesApplicable = false;

            foreach (var robot in this.Robots)
            {
                robot.Act();
            }

            foreach (var rule in this.Rules)
            {
                if (rule.IsApplicable())
                {
                    rule.Apply();
                    rulesApplicable = true;
                }
            }

        } while (rulesApplicable);
    }

    /// <summary>
    /// Protected constructor to initialize the scenario with specified parameters.
    /// </summary>
    /// <param name="map">The map associated with the scenario.</param>
    /// <param name="rules">The rules applicable to the scenario.</param>
    /// <param name="robots">The robots involved in the scenario.</param>
    protected ScenarioBase(IMap map, ReadOnlyCollection<IRule> rules, ReadOnlyObservableCollection<IRobot> robots)
    {
        // TODO: Refactor to remove circular dependency between ScenarioBase and RuleBase.
        // TODO: Rules should not be passed, rather, created based on scenario configuration.

        ArgumentNullException.ThrowIfNull(map, nameof(map));
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        ArgumentNullException.ThrowIfNull(robots, nameof(robots));
        ArgumentOutOfRangeException.ThrowIfZero(robots.Count, nameof(robots));

        this.Map = map;
        this.Rules = rules;
        this.Robots = robots;
    }
}
