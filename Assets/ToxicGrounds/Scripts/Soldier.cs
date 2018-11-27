using UnityEngine;

public class Soldier : MonoBehaviour
{
    public float Range { get; private set; }

    public Wall CurrentWall { get; private set; }

    public static Soldier Constructor(GameObject prefab, float range, Suppressor tower)
    {
        var result = MonoBehaviour.Instantiate(prefab).AddComponent<Soldier>();
        result.Range = range;
        result.transform.position = tower.Waypoint;
        return result;
    }
}
