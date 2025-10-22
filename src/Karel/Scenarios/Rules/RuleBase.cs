namespace Karel.Scenarios.Rules;

/// <summary>
/// Base class for scenario rules implementing common functionality.
/// </summary>
public abstract class RuleBase : IRule
{
    /// <inheritdoc/>
    public TimeSpan Duration { get; }

    /// <inheritdoc/>
    public virtual bool IsApplicable()
    {
        return false;
    }

    /// <inheritdoc/>
    public virtual void Apply()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Private constructor to prevent parameterless instantiation.
    /// </summary>
    private RuleBase()
    {
        // Prevent parameterless instantiation.
    }

    /// <summary>
    /// Constructor with duration parameter.
    /// </summary>
    /// <param name="duration">The duration of the rule.</param>
    protected RuleBase(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be positive or zero.");
        }

        this.Duration = duration;
    }
}