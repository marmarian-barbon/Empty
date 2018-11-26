using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.XR.WSA.Persistence;

/// <summary>
/// Представляет собой стену, соединяющую башни.
/// </summary>
public class Wall : MonoBehaviour
{
    private IWallBuilder wallBuilder;

    private IList<Suppressor> towers;

    /// <summary>
    /// Вектор, определяющий собой относительное положение и расстояние между башнями.
    /// </summary>
    public Vector3 Line { get; private set; }

    public static Wall Constructor(Suppressor tower1, Suppressor tower2, IWallBuilder wallBuilder)
    {
        var result = new GameObject().AddComponent<Wall>();
        result.transform.position = (tower1.transform.position + tower2.transform.position) / 2;
        result.Line = tower1.transform.position - tower2.transform.position;
        result.transform.rotation = Quaternion.LookRotation(result.Line);
        result.wallBuilder = wallBuilder;
        result.wallBuilder.Build(result);
        return result;
    }
}
