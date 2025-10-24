using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Karel.Common;

/// <summary>
/// Base class that implements <see cref="INotifyPropertyChanged"/> to provide property change notifications.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class ObservableBase : INotifyPropertyChanged
{
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged<T>(string propertyName, T oldValue, T newValue)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs<T>(propertyName, oldValue, newValue));
    }
}
