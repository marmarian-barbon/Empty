using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.XR.WSA.Persistence;

/// <summary>
/// Представляет собой стену, соединяющую два <see cref="Suppressor"/>.
/// </summary>
public class Wall : MonoBehaviour
{
    private IWallBuilder wallBuilder;

    private IList<Suppressor> towers;

    /// <summary>
    /// Высота стены.
    /// </summary>
    public float Height { get; private set; }

    /// <summary>
    /// Вектор, определяющий собой относительное положение и расстояние между соединенными <see cref="Suppressor"/>.
    /// </summary>
    public Vector3 Line { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tower1"></param>
    /// <param name="tower2"></param>
    /// <param name="wallBuilder"></param>
    /// <returns></returns>
    public static Wall Constructor(Suppressor tower1, Suppressor tower2, IWallBuilder wallBuilder)
    {
        var result = new GameObject().AddComponent<Wall>();
        result.Height = (tower1.Height + tower2.Height) / 2f;
        result.transform.position = (tower1.transform.position + tower2.transform.position) / 2f;
        result.Line = tower1.transform.position - tower2.transform.position;
        result.transform.rotation = Quaternion.LookRotation(result.Line);
        result.transform.localScale = new Vector3(1f, result.Height * 2, result.Line.magnitude);
        result.wallBuilder = wallBuilder;
        result.wallBuilder.Build(result);
        return result;
    }
}
