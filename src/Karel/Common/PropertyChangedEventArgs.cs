using System.Diagnostics.CodeAnalysis;

namespace Karel.Common;

/// <summary>
/// Provides data for the PropertyChanged event, including old and new values.
/// </summary>
/// <typeparam name="T">The type of the property value.</typeparam>
[ExcludeFromCodeCoverage]
public class PropertyChangedEventArgs<T> : System.ComponentModel.PropertyChangedEventArgs
{
    /// <summary>
    /// The old value of the property.
    /// </summary>
    public T OldValue { get; }

    /// <summary>
    /// The new value of the property.
    /// </summary>
    public T NewValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyChangedEventArgs{T}"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    /// <param name="oldValue">The old value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    public PropertyChangedEventArgs(string propertyName, T oldValue, T newValue)
        : base(propertyName)
    {
        this.OldValue = oldValue;
        this.NewValue = newValue;
    }
}
