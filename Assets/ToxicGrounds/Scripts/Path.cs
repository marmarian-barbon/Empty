using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Path : ICloneable
{
    private Soldier soldier;

    private Patrol patrol;

    private IList<Suppressor> towers;

    private IList<Wall> walls;

    public Path(Patrol patrol)
    {
        this.soldier = patrol.Soldier;
        this.patrol = patrol;
        this.towers = new List<Suppressor>();
        var currentWatch = patrol.Soldier.CurrentWatch;
        this.walls = new List<Wall> { currentWatch.Wall };
        this.Distance = 0f;

        var currentPosition = this.soldier.transform.position;
        var newPaths = new List<Path>();
        foreach (var tower in currentWatch.Wall.Towers)
        {
            var fromSoldierToTower = Vector3.Distance(currentPosition, tower.Waypoint) + Vector3.kEpsilon;
            var fromFirePositionToTower = float.MaxValue;
            foreach (var firePosition in currentWatch.FirePosition)
            {
                var fromAnotherFirePositionToTower = Vector3.Distance(firePosition.Value, tower.Waypoint);
                if (fromAnotherFirePositionToTower < fromSoldierToTower && fromAnotherFirePositionToTower > fromFirePositionToTower)
                {
                    fromFirePositionToTower = fromAnotherFirePositionToTower;
                }
            }

            if (fromFirePositionToTower < fromSoldierToTower)
            {
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
        this.towers = closest.towers;
        this.walls = closest.walls;
        this.Distance = closest.Distance;
    }

    private Path()
    {
    }

    public float Distance { get; private set; }

    public IEnumerator Move()
    {
        foreach (var nextTower in this.towers)
        {
            for (var currentPosition = this.soldier.transform.position;
                 Vector3.Distance(currentPosition, nextTower.Waypoint) > Vector3.kEpsilon;
                 this.soldier.transform.position = currentPosition)
            {
                currentPosition = Vector3.MoveTowards(
                    currentPosition,
                    nextTower.Waypoint,
                    Time.deltaTime * this.soldier.Speed);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public object Clone()
    {
        var newTowers = new List<Suppressor>();
        newTowers.AddRange(this.towers);

        var newWalls = new List<Wall>();
        newWalls.AddRange(this.walls);

        return new Path()
        {
            soldier = this.soldier,
            patrol = this.patrol,
            towers = newTowers,
            walls = newWalls,
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
            var alreadyInPath = this.walls.Contains(wall);
            if (alreadyInPath)
            {
                continue;
            }

            var newPath = this.Clone() as Path;
            if (newPath == null)
            {
                continue;
            }

            newPath.towers.Add(tower);
            newPath.walls.Add(wall);
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
            if (nextTower == null || newPath.towers.Contains(nextTower))
            {
                continue;
            }

            var newPaths = newPath.Add(nextTower);
            result.AddRange(newPaths);
        }

        return result;
    }
}
