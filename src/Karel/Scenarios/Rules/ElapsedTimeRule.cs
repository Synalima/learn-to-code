using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Karel.Robots;
using Karel.Scenarios.Maps;

namespace Karel.Scenarios.Rules;

/// <summary>
/// A rule that becomes applicable after a fixed duration has elapsed since construction.
/// </summary>
public sealed class ElapsedTimeRule : RuleBase
{
    private DateTime? startTime;
    private readonly TimeSpan duration;

    /// <summary>
    /// Creates a new <see cref="ElapsedTimeRule"/> and records the start time as now.
    /// </summary>
    /// <param name="map">The map associated with the rule.</param>
    /// <param name="robots">The robots collection associated with the rule.</param>
    /// <param name="duration">The timespan after which the rule becomes applicable.</param>
    public ElapsedTimeRule(IMap map, ReadOnlyCollection<IRobot> robots, TimeSpan duration)
        : base(map, robots)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(duration, TimeSpan.Zero, nameof(duration));

        this.startTime = null;
        this.duration = duration;
    }

    /// <inheritdoc/>
    public override bool IsApplicable()
    {
        if (!this.startTime.HasValue)
        {
            // Not initialized yet - not applicable.
            return false;
        }

        return (DateTime.UtcNow - this.startTime.Value) > this.duration;
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public override void Apply()
    {
        // No-op: this rule only represents a timed applicability check.
    }

    /// <inheritdoc/>
    public override void Initialize()
    {
        // Record the start time when the rule is initialized by the scenario.
        this.startTime = DateTime.UtcNow;
    }
}
