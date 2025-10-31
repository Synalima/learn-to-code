using Karel.Scenarios.Maps;

namespace Karel.Robots;

/// <summary>
/// A robot that searches the map for the objective cell and moves toward it one step at a time.
/// </summary>
public sealed class ObjectiveSeekingRobot : RobotBase
{
    /// <inheritdoc/>
    public override void Act()
    {
        var current = this.Position;
        if (current is null) return; // not initialized

        var map = current.Map;

        // Find the first occurrence of an objective cell
        ICell? objective = null;
        for (uint z = 0; z < map.Depth && objective is null; z++)
        {
            for (uint y = 0; y < map.Height && objective is null; y++)
            {
                for (uint x = 0; x < map.Width; x++)
                {
                    if (map.TryGetCell(x, y, z, out var c) && c!.IsObjective())
                    {
                        objective = c;
                        break;
                    }
                }
            }
        }

        if (objective is null) return; // nothing to seek

        // Already at objective?
        if (current.IsObjective()) return;

        // If any adjacent neighbor is the objective, move there.
        foreach (var neighbor in map.GetAllAdjacentNeighbors(current))
        {
            if (neighbor.IsObjective())
            {
                _ = this.TryMoveTo(neighbor.X, neighbor.Y, neighbor.Z, out _);
                return;
            }
        }

        // Otherwise pick a navigable adjacent neighbor that reduces Manhattan distance.
        uint bestX = current.X, bestY = current.Y, bestZ = current.Z;
        long bestDist = long.MaxValue;
        foreach (var neighbor in map.GetAllAdjacentNeighbors(current))
        {
            if (!neighbor.CanNavigate()) continue;

            long dx = (long)neighbor.X - (long)objective.X;
            long dy = (long)neighbor.Y - (long)objective.Y;
            long dz = (long)neighbor.Z - (long)objective.Z;
            long dist = Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz);

            if (dist < bestDist)
            {
                bestDist = dist;
                bestX = neighbor.X;
                bestY = neighbor.Y;
                bestZ = neighbor.Z;
            }
        }

        // If we found a better neighbor, attempt to move there.
        if (bestDist != long.MaxValue)
        {
            _ = this.TryMoveTo(bestX, bestY, bestZ, out _);
        }

    }
}

