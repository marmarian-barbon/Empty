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

    private void Start()
    {
        var tower1 = Suppressor.Constructor(new Vector3(0f, 0f, 14f), new SimpleTowerBuilder(this.towerPrefab), 3f);
        var tower2 = Suppressor.Constructor(new Vector3(3f, 0f, -5f), new SimpleTowerBuilder(this.towerPrefab), 3f);

        var wall = Wall.Constructor(tower1, tower2, new SimpleWallBuilder(this.wallPrefab));

        var soldier = Soldier.Constructor(this.soldierPrefab, 5f, tower1);
        var patrol = new Patrol(soldier, wall);

        var toxine = MonoBehaviour.Instantiate(
                this.toxinePrefab,
                new Vector3(20f, 20f, 20f),
                Quaternion.identity)
            .AddComponent<Toxine>();

        // Это нужно?
        toxine.GetComponent<Collider>().isTrigger = true;

        var coroutine = this.SmoothMovement(wall.transform.position, toxine);
        this.StartCoroutine(coroutine);
    }

    private IEnumerator SmoothMovement(Vector3 end, Toxine toxine)
    {
        var sqrRemainingDistance = (toxine.transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > Mathf.Epsilon)
        {
            var newPostion = Vector3.MoveTowards(toxine.transform.position, end, Time.deltaTime * 5);
            toxine.transform.position = newPostion;   
            sqrRemainingDistance = (toxine.transform.position - end).sqrMagnitude;

            // Вот тут что лучше использовать?
            yield return new WaitForEndOfFrame();
        }
    }
}
