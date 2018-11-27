using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Создает два <see cref="Suppressor"/> и <see cref="Wall"/> между ними; добавляет <see cref="Soldier"/> и устанавливает ему <see cref="Patrol"/>; добавляет <see cref="Toxine"/>.
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
    private GameObject toxinePrefab;

    private static IEnumerator SmoothMovement(Vector3 end, Component toxine)
    {
        var sqrRemainingDistance = (toxine.transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > Mathf.Epsilon)
        {
            var newPostion = Vector3.MoveTowards(toxine.transform.position, end, Time.deltaTime * 5);
            toxine.transform.position = newPostion;
            sqrRemainingDistance = (toxine.transform.position - end).sqrMagnitude;

            yield return new WaitForEndOfFrame();
        }
    }

    private void Start()
    {
        var tower1 = Suppressor.Constructor(new Vector3(0f, 0f, 14f), new SimpleTowerBuilder(this.towerPrefab), 3f);
        var tower2 = Suppressor.Constructor(new Vector3(3f, 0f, -5f), new SimpleTowerBuilder(this.towerPrefab), 3f);
        var tower3 = Suppressor.Constructor(new Vector3(11f, 0f, 0f), new SimpleTowerBuilder(this.towerPrefab), 3f);

        var wall12 = Wall.Constructor(tower1, tower2, new SimpleWallBuilder(this.wallPrefab));
        var wall23 = Wall.Constructor(tower2, tower3, new SimpleWallBuilder(this.wallPrefab));

        var soldier = Soldier.Constructor(this.soldierPrefab, 5f, 5f, tower1);
        var patrol = new Patrol(soldier, wall12);
        Debug.Log($"Другая стена добавлена в партулируемую территорию: {patrol.Add(wall23)}");

        var toxine = Toxine.Constructor(this.toxinePrefab, new Vector3(20f, 20f, 20f));
        var coroutine = SmoothMovement(wall12.transform.position, toxine);
        toxine.CurrentCoroutine = coroutine;
    }
}
