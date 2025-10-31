using System.Diagnostics.CodeAnalysis;

namespace Karel.Scenarios.Rules;

/// <summary>
/// Base class for scenario rules implementing common functionality.
/// </summary>
public abstract class RuleBase : IRule
{
    /// <inheritdoc/>
    /// <exception cref="NotImplementedException">Thrown when the method is not implemented in a derived class.</exception>
    [ExcludeFromCodeCoverage]
    public virtual void Apply()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public virtual void Initialize()
    {
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public virtual bool IsApplicable()
    {
        return false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RuleBase"/> class.
    /// </summary>
    public RuleBase()
    {
    }
}