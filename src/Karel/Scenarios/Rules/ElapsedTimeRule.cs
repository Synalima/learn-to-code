namespace Karel.Scenarios.Rules;

/// <summary>
/// A rule that is applicable for a fixed duration has elapsed since initialized.
/// </summary>
public sealed class ElapsedTimeRule : RuleBase
{
    private DateTime? startTime;
    private readonly TimeSpan duration;

    /// <summary>
    /// Creates a new <see cref="ElapsedTimeRule"/>.
    /// </summary>
    /// <param name="duration">The timespan for which the rule is applicable.</param>
    public ElapsedTimeRule(TimeSpan duration)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(duration, TimeSpan.Zero, nameof(duration));

        this.duration = duration;
    }

    /// <inheritdoc/>
    public override void Apply()
    {
        // No action needed when the rule is applied.
    }

    /// <inheritdoc/>
    public override bool IsApplicable()
    {
        if (!this.startTime.HasValue)
        {
            // Not initialized yet - not applicable.
            return false;
        }

        return (DateTime.UtcNow - startTime.Value).TotalMilliseconds <= duration.TotalMilliseconds;
    }

    /// <inheritdoc/>
    public override void Initialize()
    {
        // Record the start time when the rule is initialized by the scenario.
        this.startTime = DateTime.UtcNow;
    }
}
