using System.Collections.ObjectModel;
using Karel.Robots;
using Karel.Scenarios.Maps;
using Karel.Scenarios.Rules;

namespace Karel.Scenarios;

/// <summary>
/// A scenario that places two objective-seeking robots on a flat map and stops when a timed rule becomes applicable.
/// </summary>
public class TimedObjectiveSeekingScenario : ScenarioBase
{
    /// <summary>
    /// Creates a new timed scenario using a generated FlatMap and two ObjectiveSeekingRobot instances.
    /// </summary>
    /// <param name="width">Map width (must be &gt;= 2).</param>
    /// <param name="height">Map height (must be &gt;= 2).</param>
    /// <param name="duration">Duration after which the timed rule becomes applicable.</param>
    public TimedObjectiveSeekingScenario(uint width, uint height, TimeSpan duration)
        : this(new FlatMap(width, height), CreateDefaultRobots(), duration)
    {
    }

    /// <summary>
    /// Testable constructor allowing injection of a map and robots (useful for unit tests).
    /// </summary>
    /// <param name="map">The map for the scenario.</param>
    /// <param name="robots">The robots for the scenario.</param>
    /// <param name="duration">Duration after which the timed rule becomes applicable.</param>
	public TimedObjectiveSeekingScenario(IMap map, ReadOnlyObservableCollection<IRobot> robots, TimeSpan duration)
		: base(map, robots)
	{
		// Create the timed rule but defer initialization until Run() begins.
		this.SetRules(new ReadOnlyCollection<IRule>([new ElapsedTimeRule(duration)]));

		// Ensure robots have initial positions if possible: prefer StartPosition cells.
		foreach (var robot in this.Robots)
		{
			if (robot.Position == null)
			{
				// Try to find a start cell
				for (uint z = 0; z < map.Depth; z++)
				{
					for (uint y = 0; y < map.Height; y++)
					{
						for (uint x = 0; x < map.Width; x++)
						{
							if (map.TryGetCell(x, y, z, out var c) && c!.IsStartPosition())
							{
								robot.Initialize(c!);
								goto NextRobot;
							}
						}
					}
				}

				// If no start found, initialize to (0,0,0) if available
				if (map.TryGetCell(0, 0, 0, out var fallback))
				{
					robot.Initialize(fallback!);
				}
			}

		NextRobot: ;
		}
	}

	private static ReadOnlyObservableCollection<IRobot> CreateDefaultRobots()
	{
		return new ReadOnlyObservableCollection<IRobot>([new ObjectiveSeekingRobot(), new ObjectiveSeekingRobot()]);
	}
}
