using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Солдат, который будет убивать врагов в зоне действия его <see cref="Patrol"/>.
/// </summary>
public class Soldier : MonoBehaviour
{
    public float Speed { get; private set; }

    /// <summary>
    /// Дальность стрельбы.
    /// </summary>
    public float Range { get; private set; }

    /// <summary>
    /// <see cref="Wall"/>, на которой сейчас стоит.
    /// </summary>
    public Watch CurrentWatch { get; private set; }

    public Patrol Patrol { get; private set; }

    public IEnumerator CurrentRoutine { get; private set; }

    public static Soldier Constructor(GameObject prefab, float range, float speed, Suppressor tower)
    {
        var result = MonoBehaviour.Instantiate(prefab).AddComponent<Soldier>();
        result.Range = range;
        result.Speed = speed;
        result.transform.position = tower.Waypoint;
        return result;
    }

    public Patrol SetPatrol(Wall wall)
    {
        // TODO чтобы можно было начать с wall, которая не находится рядом с начальным положением
        var isNear = wall.Towers.Any(tower => Vector3.Distance(tower.Waypoint, this.transform.position) <= Vector3.kEpsilon);
        if (!isNear)
        {
            return null;
        }

        this.Patrol = new Patrol(this, wall);
        this.CurrentWatch = this.Patrol.Watches.First();
        return this.Patrol;
    }

    /// <summary>
    /// Заставляет посмотреть, нет ли другой цели для атаки
    /// </summary>
    public void ReTarget()
    {
        var path = new Path(this.Patrol);
        Debug.Log($"Nearest target is {path.Distance} away");
        this.CurrentRoutine = path.Move();
        this.StartCoroutine(this.CurrentRoutine);
        this.CurrentWatch = this.Patrol.Watches.FirstOrDefault(watch => watch.Wall == path.Walls.Last());
    }
}
