namespace Karel.Scenarios.Rules;

using System;

/// <summary>
/// Defines a rule that can be applied within a Karel scenario.
/// </summary>
public interface IRule
{
    /// <summary>
    /// Gets the duration of the rule application.
    /// </summary>
    TimeSpan Duration { get; }

    /// <summary>
    /// Determines whether the rule is applicable in the current context.
    /// </summary>
    /// <returns>
    /// True if the rule can be applied; otherwise, false.
    /// </returns>
    bool IsApplicable();
    
    /// <summary>
    /// Applies the rule.
    /// </summary>
    void Apply();
}
