using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Солдат, который будет убивать врагов в зоне действия его <see cref="Patrol"/>.
/// </summary>
public class Soldier : MonoBehaviour
{
    /// <summary>
    /// Дальность стрельбы.
    /// </summary>
    public float Range { get; private set; }

    /// <summary>
    /// <see cref="Wall"/>, на которой сейчас стоит.
    /// </summary>
    public Wall CurrentWall { get; private set; }

    public static Soldier Constructor(GameObject prefab, float range, Suppressor tower)
    {
        var result = MonoBehaviour.Instantiate(prefab).AddComponent<Soldier>();
        result.Range = range;
        result.transform.position = tower.Waypoint;
        return result;
    }

    /// <summary>
    /// Заставляет посмотреть, нет ли другой цели для атаки
    /// </summary>
    public void ReTarget()
    {
        // TODO логику
    }
}
