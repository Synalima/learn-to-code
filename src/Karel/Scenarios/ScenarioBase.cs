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
    protected ReadOnlyCollection<IRule> Rules { get; private set; }

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
        this.Rules.ToList().ForEach(r => r.Initialize());

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
    /// <param name="robots">The robots involved in the scenario.</param>
    /// <exception cref="ArgumentNullException">Thrown when map or robots is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when there are no robots in the scenario.</exception>
    /// <remarks>
    /// Derived implementations are responsible for constructing and assigning the Rules collection.
    /// </remarks>
    protected ScenarioBase(IMap map, ReadOnlyObservableCollection<IRobot> robots)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        ArgumentNullException.ThrowIfNull(robots, nameof(robots));
        ArgumentOutOfRangeException.ThrowIfZero(robots.Count, nameof(robots));

        this.Map = map;
        this.Robots = robots;
        this.Rules = new ReadOnlyCollection<IRule>(Array.Empty<IRule>());
    }

    /// <summary>
    /// Allows derived scenario implementations to set the rules collection during construction.
    /// </summary>
    /// <param name="rules">The rules to assign to the scenario.</param>
    /// <exception cref="ArgumentNullException">Thrown when rules is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when there are no rules in the scenario.</exception>
    /// <exception cref="InvalidOperationException">Thrown when rules have already been assigned.</exception>
    protected void SetRules(ReadOnlyCollection<IRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        ArgumentOutOfRangeException.ThrowIfZero(rules.Count, nameof(rules));

        if (this.Rules.Count != 0)
        {
            throw new InvalidOperationException("Rules have already been assigned and may only be set once.");
        }

        this.Rules = rules;
    }
}
