﻿using System.Collections;
using System.Linq;

using UnityEngine;

/// <summary>
/// Солдат, который будет убивать врагов в зоне действия его <see cref="Patrol"/>.
/// </summary>
public class Soldier : MonoBehaviour
{
    private IEnumerator currentRoutine;

    /// <summary>
    /// Дальность стрельбы.
    /// </summary>
    public float Range { get; private set; }

    /// <summary>
    /// <see cref="Wall"/>, на которой сейчас стоит.
    /// </summary>
    public Wall CurrentWall { get; private set; }

    public IEnumerator CurrentRoutine
    {
        get
        {
            return this.currentRoutine;
        }

        private set
        {
            if (this.currentRoutine != null)
            {
                this.StopCoroutine(this.currentRoutine);
            }

            this.currentRoutine = value;
            if (this.currentRoutine != null)
            {
                this.StartCoroutine(this.currentRoutine);
            }
        }
    }

    private Actions Actions { get; set; }

    private Patrol Patrol { get; set; }

    private float Speed { get; set; }

    public static Soldier Constructor(GameObject prefab, float range, float speed, Suppressor tower)
    {
        var result = MonoBehaviour.Instantiate(prefab).AddComponent<Soldier>();
        result.Range = range;
        result.Speed = speed;
        result.transform.position = tower.Waypoint;
        result.Actions = result.gameObject.GetComponent<Actions>();
        result.gameObject.GetComponent<PlayerController>().SetArsenal("Rifle");
        
        return result;
    }

    /// <summary>
    /// Пытается установить <see cref="Patrol"/> с начальным <see cref="Watch"/> на <seealso cref="wall"/>.
    /// </summary>
    /// <param name="wall"><see cref="Wall"/> для первого <seealso cref="Watch"/> в новом <see cref="Patrol"/></param>
    /// <returns></returns>
    public Patrol SetPatrol(Wall wall)
    {
        var isNear = wall.Towers.Any(
            tower => Vector3.Distance(tower.Waypoint, this.transform.position) <= Vector3.kEpsilon);
        if (!isNear)
        {
            return null;
        }

        this.Patrol = new Patrol(this, wall);
        this.CurrentWall = this.Patrol.Watches.First().Wall;
        return this.Patrol;
    }

    /// <summary>
    /// Заставляет посмотреть, нет ли другой цели для атаки.
    /// </summary>
    public void ReTarget()
    {
        var path = new Path(this.Patrol);
        Debug.Log($"Nearest target is {path.Distance} away");
        this.CurrentRoutine = this.Move(path);
    }

    private void TryShoot()
    {
        if (this.CurrentRoutine != null)
        {
            this.StopCoroutine(this.CurrentRoutine);
        }

        var watch = this.Patrol.WatchOf(this.CurrentWall);
        this.CurrentRoutine = watch != null ? this.Chase(watch) : null;
    }

    private IEnumerator Chase(Watch watch)
    {
        if (watch.FirePosition.Count == 0)
        {
            this.Actions.Stay();
            this.CurrentRoutine = null;
            yield break;
        }

        var firePosition = watch.FirePosition.First();
        this.Actions.Run();
        do
        {
            if (!firePosition.Key.gameObject.activeSelf)
            {
                if (watch.FirePosition.Count == 0)
                {
                    this.Actions.Stay();
                    this.currentRoutine = null;
                    yield break;
                }

                firePosition = watch.FirePosition.First();
            }

            foreach (var target in watch.FirePosition)
            {
                if (Vector3.Distance(this.transform.position, target.Value) < Vector3.Distance(this.transform.position, firePosition.Value))
                {
                    firePosition = target;
                }
            }

            var newRotation = this.transform.rotation;
            newRotation.SetLookRotation(firePosition.Value - this.transform.position, Vector3.up);
            this.transform.rotation = newRotation;
            this.transform.position = Vector3.MoveTowards(this.transform.position, firePosition.Value, Time.deltaTime * this.Speed);
            yield return new WaitForEndOfFrame();
        }
        while (Vector3.Distance(this.transform.position, firePosition.Key.transform.position) > this.Range + firePosition.Key.Size);

        this.CurrentRoutine = this.Shoot(firePosition.Key, watch);
    }

    private IEnumerator Shoot(Toxin toxin, Watch watch)
    {
        this.Actions.Attack();
        
        while (true)
        {
            // TODO стрельбу
            Debug.DrawLine(this.transform.position, toxin.transform.position, Color.magenta);

            var newRotation = this.transform.rotation;
            var lookAt = toxin.transform.position - this.transform.position;
            lookAt.y = 0;
            newRotation.SetLookRotation(lookAt, Vector3.up);
            this.transform.rotation = newRotation;
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, watch.FirePosition[toxin], Time.deltaTime * this.Speed);

            toxin.Health = toxin.Health - (Time.deltaTime * 30);

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Двигаться по указанному <seealso cref="path"/>.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator Move(Path path)
    {
        for (var i = 0; i < path.Towers.Count; i++)
        {
            var nextTower = path.Towers[i];

            var newRotation = this.transform.rotation;
            newRotation.SetLookRotation(nextTower.Waypoint - this.transform.position, Vector3.up);
            this.transform.rotation = newRotation;

            this.transform.rotation.SetLookRotation(nextTower.Waypoint, Vector3.up);
            this.Actions.Run();
            for (var currentPosition = this.transform.position;
                 Vector3.Distance(currentPosition, nextTower.Waypoint) > Vector3.kEpsilon;
                 this.transform.position = currentPosition)
            {
                currentPosition = Vector3.MoveTowards(
                    currentPosition,
                    nextTower.Waypoint,
                    Time.deltaTime * this.Speed);
                yield return new WaitForEndOfFrame();
            }

            this.CurrentWall = path.Walls[i + 1];
        }

        this.TryShoot();
    }
}