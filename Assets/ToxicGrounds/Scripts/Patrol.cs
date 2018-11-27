using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Представляет собой всю территорию, охраняемую <see cref="Soldier"/>.
/// </summary>
public class Patrol
{
    public Patrol(Soldier soldier, Wall wall)
    {
        this.Soldier = soldier;
        this.Watches = new HashSet<Watch> { Watch.Constructor(wall, this.Soldier) };
        this.Towers = new HashSet<Suppressor>();
        foreach (var tower in wall.Towers)
        {
            this.Towers.Add(tower);
        }
    }

    public ISet<Watch> Watches { get; }

    public ISet<Suppressor> Towers { get; }

    public Soldier Soldier { get; }

    /// <summary>
    /// Пытается присоединить новый кусок территории.
    /// </summary>
    /// <param name="wall"></param>
    /// <returns></returns>
    public bool Add(Wall wall)
    {
        if (this.Watches.Any(watch => wall == watch.Wall))
        {
            return false;
        }

        var connected = wall.Towers.Aggregate(false, (current, tower) => current | this.Towers.Contains(tower));
        if (!connected)
        {
            return false;
        }

        foreach (var tower in wall.Towers)
        {
            this.Towers.Add(tower);
        }

        this.Watches.Add(Watch.Constructor(wall, this.Soldier));
        return true;
    }
}


