using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

/// <summary>
/// Представляет собой башню.
/// </summary>
public class Suppressor : MonoBehaviour
{
    private ITowerBuilder towerBuilder;

    /// <summary>
    /// Высота башни.
    /// </summary>
    public float Height { get; private set; }

    /// <summary>
    /// Точка, к которой могут идти TODO.
    /// </summary>
    public Vector3 Waypoint { get; private set; }

    public ICollection<Wall> ConnectedWalls { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="position">Нижняя точка башни.</param>
    /// <param name="towerBuilder"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Suppressor Constructor(Vector3 position, ITowerBuilder towerBuilder, float height)
    {
        var result = new GameObject().AddComponent<Suppressor>();
        result.Height = height;
        result.transform.position = position + (result.transform.up * result.Height);
        result.Waypoint = result.transform.position + (result.transform.up * result.Height);
        var scale = result.transform.localScale;
        scale.Scale(new Vector3(1f, result.Height, 1f));
        result.transform.localScale = scale;
        result.towerBuilder = towerBuilder;
        result.towerBuilder.Build(result);
        result.ConnectedWalls = new List<Wall>();
        return result;
    }
}
