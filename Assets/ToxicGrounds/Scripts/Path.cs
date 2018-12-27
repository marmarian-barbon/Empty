using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Path : ICloneable
{
    private Soldier soldier;

    private Patrol patrol;

    /// <summary>
    /// <see cref="Path"/> до любой ближайшей цели в этом <seealso cref="patrol"/>.
    /// </summary>
    /// <param name="patrol"></param>
    public Path(Patrol patrol)
    {
        this.soldier = patrol.Soldier;
        this.patrol = patrol;
        this.Towers = new List<Suppressor>();
        var currentWall = patrol.Soldier.CurrentWall;
        this.Walls = new List<Wall> { currentWall };
        this.Distance = 0f;

        var currentPosition = this.soldier.transform.position;
        var newPaths = new List<Path>();
        var currentWatch = this.patrol.WatchOf(currentWall);

        foreach (var tower in currentWall.Towers)
        {
            var fromSoldierToTower = Vector3.Distance(currentPosition, tower.Waypoint) + Vector3.kEpsilon;
            var fromFirePositionToTower = float.MinValue;

            // Watch вообще есть => изменится fromFirePositionToTower
            if (currentWatch != null)
            {
                foreach (var firePosition in currentWatch.FirePosition)
                {
                    var fromAnotherFirePositionToTower = Vector3.Distance(firePosition.Value, tower.Waypoint);
                    if (fromAnotherFirePositionToTower < fromSoldierToTower
                        && fromAnotherFirePositionToTower > fromFirePositionToTower)
                    {
                        fromFirePositionToTower = fromAnotherFirePositionToTower;
                    }
                }
            }
            
            if (fromFirePositionToTower > 0f && fromFirePositionToTower < fromSoldierToTower)
            {
                // fromFirePositionToTower стал меньше, чем fromSoldierToTower => Watch есть и вообще стоит прекратить поиск в том направлении
                var newPath = this.Clone() as Path;
                if (newPath == null)
                {
                    throw new Exception();
                }

                newPath.Distance += fromSoldierToTower - fromFirePositionToTower;
                newPaths.Add(newPath);
            }
            else
            {
                // не стал => ищем дальше
                var newPath = this.Clone() as Path;
                if (newPath == null)
                {
                    throw new Exception();
                }

                newPath.Distance += fromSoldierToTower;
                newPaths.AddRange(newPath.Add(tower));
            }
        }


        if (newPaths.Count <= 0)
        {
            return;
        }

        var closest = newPaths.OrderBy(path => path.Distance).First();
        this.Towers = closest.Towers;
        this.Walls = closest.Walls;
        this.Distance = closest.Distance;
    }

    private Path()
    {
    }

    public IList<Suppressor> Towers { get; private set; }

    public IList<Wall> Walls { get; private set; }

    public float Distance { get; private set; }

    public object Clone()
    {
        var newTowers = new List<Suppressor>();
        newTowers.AddRange(this.Towers);

        var newWalls = new List<Wall>();
        newWalls.AddRange(this.Walls);

        return new Path()
        {
            soldier = this.soldier,
            patrol = this.patrol,
            Towers = newTowers,
            Walls = newWalls,
            Distance = this.Distance
        };
    }

    /// <summary>
    /// Возвращает возможные пути, которые могли бы получиться при добавлении новой <seealso cref="tower"/>.
    /// </summary>
    /// <param name="tower"></param>
    /// <returns></returns>
    private List<Path> Add(Suppressor tower)
    {
        var result = new List<Path>();
        foreach (var wall in tower.ConnectedWalls)
        {
            var alreadyInPath = this.Walls.Contains(wall);
            if (alreadyInPath)
            {
                continue;
            }

            var newPath = this.Clone() as Path;
            if (newPath == null)
            {
                continue;
            }

            newPath.Towers.Add(tower);
            newPath.Walls.Add(wall);
            newPath.Distance += wall.Line.magnitude;

            var wallWatch = this.patrol.Watches.FirstOrDefault(watch => watch.Wall == wall);
            if (wallWatch != null)
            {
                var haveTarget = wallWatch.FirePosition.Count > 0;
                if (haveTarget)
                {
                    var nearestDistance = wallWatch.FirePosition.Select(position => Vector3.Distance(tower.Waypoint, position.Value)).Concat(new[] { float.MaxValue }).Min();

                    newPath.Distance += nearestDistance;
                    result.Add(newPath);
                    continue;
                }
            }

            var nextTower = wall.Towers.FirstOrDefault(suppressor => suppressor != tower);
            if (nextTower == null || newPath.Towers.Contains(nextTower))
            {
                continue;
            }

            var newPaths = newPath.Add(nextTower);
            result.AddRange(newPaths);
        }

        return result;
    }
}
