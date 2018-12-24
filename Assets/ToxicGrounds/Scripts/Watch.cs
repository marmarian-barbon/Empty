using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Представляет собой наблюдение за отдельной <see cref="Wall"/>.
/// </summary>
public class Watch : MonoBehaviour
{
    /// <summary>
    /// Оптимальная позиция на <see cref="Wall"/> для стрельбы <see cref="Soldier"/>.
    /// </summary>
    public IDictionary<Toxin, Vector3> FirePosition { get; private set; }

    /// <summary>
    /// <see cref="Wall"/>, за которой идет наблюдение.
    /// </summary>
    public Wall Wall { get; private set; }

    /// <summary>
    /// <see cref="Soldier"/>, который осуществляет наблюдение.
    /// </summary>
    public Soldier Soldier { get; private set; }

    public static Watch Constructor(Wall wall, Soldier soldier)
    {
        var result = new GameObject().AddComponent<Watch>();
        result.FirePosition = new Dictionary<Toxin, Vector3>();
        result.transform.position = wall.transform.position;
        result.transform.rotation = wall.transform.rotation;
        result.transform.parent = wall.transform;
        result.Wall = wall;
        result.Soldier = soldier;
        var collider = result.gameObject.AddComponent<CapsuleCollider>();
        collider.direction = 2;
        collider.isTrigger = true;
        collider.transform.parent = wall.transform;
        collider.radius = soldier.Range;
        collider.height = wall.Line.magnitude + (collider.radius * 2);
        return result;
    }

    /// <summary>
    /// Расчитывает Оптимальную позицию на <seealso cref="Wall"/> для стрельбы по <seealso cref="enemy"/>.
    /// </summary>
    /// <returns></returns>
    private Vector3 OptimalPosition(Toxin enemy)
    {
        var towers = this.Wall.Towers;
        var tower0ToTarget = enemy.transform.position - this.Wall.Towers[0].Waypoint;
        var tower1ToTarget = enemy.transform.position - this.Wall.Towers[1].Waypoint;
        var tower0ToTower1 = this.Wall.Towers[1].Waypoint - this.Wall.Towers[0].Waypoint;

        // Угол тупой => цель находится на "крышке" (капсули), прилегающей к башне[0] => лучше стоять ровно на башне[0]
        if (Vector3.Angle(tower0ToTarget, tower0ToTower1) > 90f)
        {
            return towers[0].Waypoint;
        }

        // Угол тупой => цель находится на "крышке" (капсули), прилегающей к башне[1] => лучше стоять ровно на башне[1]
        if (Vector3.Angle(tower1ToTarget, -tower0ToTower1) > 90f)
        {
            return towers[1].Waypoint;
        }

        // Углы острые => оптимальная позиция будет лежать где-то на основной линии стены
        return towers[0].Waypoint + Vector3.Project(tower0ToTarget, tower0ToTower1);
    }

    private void OnTriggerEnter(Collider other)
    {
        var toxin = other.gameObject.GetComponent<Toxin>();
        if (toxin == null)
        {
            return;
        }

        toxin.TriggeredBy.Add(this);
        var optimalPosition = this.OptimalPosition(toxin);
        this.FirePosition.Add(toxin, optimalPosition);
        this.Soldier.ReTarget();
    }

    private void OnTriggerStay(Collider other)
    {
        var toxine = other.gameObject.GetComponent<Toxin>();
        if (toxine == null)
        {
            return;
        }

        var optimalPosition = this.OptimalPosition(toxine);
        this.FirePosition[toxine] = optimalPosition;
    }

    public void CheckTarget(Toxin target)
    {
        this.FirePosition.Remove(target);
        this.Soldier.ReTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var target in this.FirePosition)
        {
            if (target.Key == null)
            {
                this.FirePosition.Remove(target);
            }
        }

        var toxine = other.gameObject.GetComponent<Toxin>();
        if (toxine == null)
        {
            return;
        }

        this.FirePosition.Remove(toxine);
        Debug.Log("Its gone");
        this.Soldier.ReTarget();
    }
}
