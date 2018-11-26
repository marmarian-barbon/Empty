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
        var tower1 = Suppressor.Constructor(new Vector3(0f, 0f, 14f), new SimpleTowerBuilder(this.towerPrefab), 3f);
        var tower2 = Suppressor.Constructor(new Vector3(3f, 0f, -5f), new SimpleTowerBuilder(this.towerPrefab), 3f);

        var wall = Wall.Constructor(tower1, tower2, new SimpleWallBuilder(this.wallPrefab));
    }
}
