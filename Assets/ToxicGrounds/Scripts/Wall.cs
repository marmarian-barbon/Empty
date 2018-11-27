using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.WSA.Persistence;

/// <summary>
/// Представляет собой стену, соединяющую два <see cref="Suppressor"/>.
/// </summary>
public class Wall : MonoBehaviour
{
    private IWallBuilder wallBuilder;

    private IReadOnlyList<Suppressor> towers;

    /// <summary>
    /// Две <see cref="Suppressor"/>, которые соединяет эта <see cref="Wall"/>.
    /// </summary>
    public IReadOnlyList<Suppressor> Towers
    {
        get
        {
            return this.towers.ToList();
        }

        private set
        {
            if (value.Count() == 2)
            {
                this.towers = value;
            }
        }
    }

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
        tower1.ConnectedWalls.Add(result);
        tower2.ConnectedWalls.Add(result);
        result.Towers = new[] { tower1, tower2 };
        result.Height = (tower1.Height + tower2.Height) / 2f;
        result.transform.position = (tower1.transform.position + tower2.transform.position) / 2f;
        result.Line = tower1.transform.position - tower2.transform.position;
        result.transform.rotation = Quaternion.LookRotation(result.Line);
        var scale = result.transform.localScale;
        scale.Scale(new Vector3(1f, result.Height * 2, result.Line.magnitude));
        result.transform.localScale = scale;
        result.wallBuilder = wallBuilder;
        result.wallBuilder.Build(result);
        return result;
    }
}
