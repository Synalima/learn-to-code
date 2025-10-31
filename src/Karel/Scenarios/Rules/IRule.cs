namespace Karel.Scenarios.Rules;

/// <summary>
/// Defines a rule that can be applied within a Karel scenario.
/// </summary>
public interface IRule
{
    /// <summary>
    /// Applies the rule.
    /// </summary>
    void Apply();

    /// <summary>
    /// Initializes the rule before application.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Determines whether the rule is applicable in the current context.
    /// </summary>
    /// <returns>
    /// True if the rule can be applied; otherwise, false.
    /// </returns>
    bool IsApplicable();
}
