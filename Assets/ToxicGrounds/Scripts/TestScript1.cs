using UnityEngine;

/// <summary>
/// Создает два <see cref="Suppressor"/> и <see cref="Wall"/> между ними.
/// </summary>
public class TestScript1 : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private GameObject towerPrefab;

    private void Start()
    {
        var tower1 = Suppressor.Constructor(new Vector3(0, -4, 14), new SimpleTowerBuilder(this.towerPrefab));
        var tower2 = Suppressor.Constructor(new Vector3(3, 2, -5), new SimpleTowerBuilder(this.towerPrefab));

        var wall = Wall.Constructor(tower1, tower2, new SimpleWallBuilder(this.wallPrefab));
    }
}
