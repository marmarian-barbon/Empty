using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Path : ICloneable
{
    private IList<Suppressor> towers;

    private IList<Wall> walls;

    private Patrol patrol;

    public Path(Patrol patrol)
    {
        this.patrol = patrol;
        this.towers = new List<Suppressor>();
        this.walls = new List<Wall> { patrol.Soldier.CurrentWatch.Wall };
    }


    /// <summary>
    /// Возвращает возможные пути, которые могли бы получиться при добавлении новой <seealso cref="tower"/>.
    /// </summary>
    /// <param name="tower"></param>
    /// <returns></returns>
    private List<Path> AddTower(Suppressor tower)
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
            if (nextTower == null)
            {
                continue;
            }

            var newPaths = newPath.AddTower(nextTower);
            result.AddRange(newPaths);
        }

        return result;
    }

    public Path(Vector3 currentPosition, Suppressor firstTower)
    {
        this.Distance = Vector3.Distance(currentPosition, firstTower.Waypoint);
        this.towers = new List<Suppressor> { firstTower };
    }

    private Path()
    {
    }

    public float Distance { get; private set; }

    public bool TryAdd(Suppressor nextTower)
    {
        if (this.towers.Contains(nextTower))
        {
            return false;
        }

        var last = this.towers.Last();
        var connected = false;
        foreach (var wall in last.ConnectedWalls)
        {
            if (wall.Towers.Any(tower => tower == nextTower))
            {
                connected = true;
            }
        }

        if (!connected)
        {
            return false;
        }

        this.Distance += Vector3.Distance(last.Waypoint, nextTower.Waypoint);
        this.towers.Add(nextTower);
        return true;
    }

    public IEnumerator GoNext(Soldier soldier)
    {
        foreach (var nextTower in this.towers)
        {
            for (var currentPosition = soldier.transform.position;
                 Vector3.Distance(currentPosition, nextTower.Waypoint) > Vector3.kEpsilon;
                 soldier.transform.position = currentPosition)
            {
                currentPosition = Vector3.MoveTowards(
                    currentPosition,
                    nextTower.Waypoint,
                    Time.deltaTime * soldier.Speed);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public object Clone()
    {
        var newList = new List<Suppressor>();
        newList.AddRange(this.towers);
        return new Path() { Distance = this.Distance, towers = newList };
    }
}
