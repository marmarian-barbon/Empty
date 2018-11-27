﻿using UnityEngine;

/// <summary>
/// Представляет собой наблюдение за отдельной <see cref="Wall"/>.
/// </summary>
public class Watch : MonoBehaviour
{
    /// <summary>
    /// <see cref="Wall"/>, за которой идет наблюдение.
    /// </summary>
    public Wall Wall { get; private set; }

    public static Watch Constructor(Wall wall, float range)
    {
        var result = new GameObject().AddComponent<Watch>();
        result.transform.position = wall.transform.position;
        result.transform.parent = wall.transform;
        result.Wall = wall;
        var collider = result.gameObject.AddComponent<CapsuleCollider>();
        collider.isTrigger = true;
        collider.transform.parent = wall.transform;

        // TODO допилить поворот на нужный угол
        collider.radius = range;
        collider.height = wall.Line.magnitude;

        return result;
    }
}
