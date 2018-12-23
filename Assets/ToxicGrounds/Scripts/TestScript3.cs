using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Создает три <see cref="Suppressor"/> и  три <see cref="Wall"/> между ними, образуя треугольник; добавляет <see cref="Soldier"/> и устанавливает ему <see cref="Patrol"/>, состоящий только из двух <see cref="Watch"/>, не добавляя последнюю <see cref="Wall"/>; добавляет два <see cref="Toxin"/>. Таким образом, <see cref="Soldier"/> будет переходить между <see cref="Watch"/>, которые нашли <see cref="Toxin"/> (если их двигать в редакторе).
/// </summary>
public class TestScript3 : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private GameObject soldierPrefab;

    [SerializeField]
    private GameObject toxin;

    [SerializeField]
    private GameObject mainPlane;

    private static IEnumerator SmoothMovement(Vector3 end, Component toxin)
    {
        var sqrRemainingDistance = (toxin.transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > Mathf.Epsilon)
        {
            var newPostion = Vector3.MoveTowards(toxin.transform.position, end, Time.deltaTime * 5);
            toxin.transform.position = newPostion;
            sqrRemainingDistance = (toxin.transform.position - end).sqrMagnitude;

            yield return new WaitForEndOfFrame();
        }
    }

    private void Start()
    {
        WorldComponents.MainPlane = this.mainPlane;
        WorldComponents.TowerPrefab = this.towerPrefab;
        WorldComponents.WallPrefab = this.wallPrefab;

        var tower0 = Suppressor.Constructor(new Vector3(0f, 0f, 6f), new SimpleTowerBuilder(this.towerPrefab), 2f);
        var tower1 = Suppressor.Constructor(new Vector3(6f, 0f, -4f), new SimpleTowerBuilder(this.towerPrefab), 2f);
        var tower2 = Suppressor.Constructor(new Vector3(-6f, 0f, -4f), new SimpleTowerBuilder(this.towerPrefab), 2f);

        var wall01 = Wall.Constructor(tower0, tower1, new SimpleWallBuilder(this.wallPrefab));
        var wall12 = Wall.Constructor(tower1, tower2, new SimpleWallBuilder(this.wallPrefab));
        var wall20 = Wall.Constructor(tower2, tower0, new SimpleWallBuilder(this.wallPrefab));

        var soldier = Soldier.Constructor(this.soldierPrefab, 5f, 5f, tower0);
        var patrol = soldier.SetPatrol(wall01);
        Debug.Log($"Другая стена добавлена в партулируемую территорию: {patrol.Add(wall12)}");
        var toxin0 = Toxin.Constructor(this.toxin, new Vector3(0f, 10f, 6f));
        var toxin1 = Toxin.Constructor(this.toxin, new Vector3(-6f, 10f, -4f));
    }
}
