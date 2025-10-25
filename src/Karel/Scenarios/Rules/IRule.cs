namespace Karel.Scenarios.Rules;

using Karel.Robots;
using Karel.Scenarios.Maps;

/// <summary>
/// Defines a rule that can be applied within a Karel scenario.
/// </summary>
public interface IRule
{
    /// <summary>
    /// Determines whether the rule is applicable in the current context.
    /// </summary>
    /// <returns>
    /// <param name="map">The map on which the rule is applied.</param>
    /// <param name="robots">The collection of robots involved in the scenario.</param>
    /// True if the rule can be applied; otherwise, false.
    /// </returns>
    bool IsApplicable(IMap map, IReadOnlyCollection<IRobot> robots);
    
    /// <summary>
    /// Applies the rule.
    /// </summary>
    /// <param name="map">The map on which the rule is applied.</param>
    /// <param name="robots">The collection of robots involved in the scenario.</param>
    /// <returns>True if the rule was applied; otherwise, false.</returns>
    bool Apply(IMap map, IReadOnlyCollection<IRobot> robots);
}
