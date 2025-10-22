using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Karel;

/// <summary>
/// Main program class for Karel application.
/// </summary>
[ExcludeFromCodeCoverage]
public class Program
{
    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    /// <param name="args">
    /// Command-line arguments.
    /// </param>
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        // services.AddSingleton<MapFactory>();
        // services.AddTransient<IMap>(sp => sp.GetRequiredService<MapFactory>().CreateMap(5, 5, new[] { (2, 2) }));
        // services.AddTransient<IRobot, Robot>();
    }
}