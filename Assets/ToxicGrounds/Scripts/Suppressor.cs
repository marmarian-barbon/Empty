using UnityEngine;

/// <summary>
/// Представляет собой башню.
/// </summary>
public class Suppressor : MonoBehaviour
{
    private ITowerBuilder towerBuilder;

    public static Suppressor Constructor(Vector3 position, ITowerBuilder towerBuilder)
    {
        var result = new GameObject().AddComponent<Suppressor>();
        result.gameObject.transform.position = position;
        result.towerBuilder = towerBuilder;
        result.towerBuilder.Build(result);
        return result;
    }
}
