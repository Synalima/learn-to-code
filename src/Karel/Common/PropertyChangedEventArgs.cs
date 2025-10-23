using System.Diagnostics.CodeAnalysis;

namespace Karel.Common;

/// <summary>
/// Provides data for the PropertyChanged event, including old and new values.
/// </summary>
/// <typeparam name="T">The type of the property value.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="PropertyChangedEventArgs{T}"/> class.
/// </remarks>
/// <param name="propertyName">The name of the property that changed.</param>
/// <param name="oldValue">The old value of the property.</param>
/// <param name="newValue">The new value of the property.</param>
[ExcludeFromCodeCoverage]
public class PropertyChangedEventArgs<T>(string propertyName, T oldValue, T newValue) : System.ComponentModel.PropertyChangedEventArgs(propertyName)
{
    /// <summary>
    /// The old value of the property.
    /// </summary>
    public T OldValue { get; } = oldValue;

    /// <summary>
    /// The new value of the property.
    /// </summary>
    public T NewValue { get; } = newValue;
}
