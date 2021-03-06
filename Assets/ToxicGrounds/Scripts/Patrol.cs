﻿using System.Collections.Generic;
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
        var firstWatch = Watch.Constructor(wall, this.Soldier);
        this.Watches = new HashSet<Watch> { firstWatch };
        this.Towers = new HashSet<Suppressor>();
        foreach (var tower in wall.Towers)
        {
            this.Towers.Add(tower);
        }
    }

    public ISet<Watch> Watches { get; }

    public ISet<Suppressor> Towers { get; }

    public Soldier Soldier { get; }

    public void Destroy()
    {
        foreach (var watch in Watches)
        {
            MonoBehaviour.Destroy(watch);
        }
    }

    public Watch WatchOf(Wall wall)
    {
        return this.Watches.FirstOrDefault(watch => watch.Wall == wall);
    }

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


