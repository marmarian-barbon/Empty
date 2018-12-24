using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.Serialization;

using Vuforia;

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

    [SerializeField]
    private GameObject testPrefab;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("yes");
        }
    }

    private IEnumerator GenerateToxins(float timeInterval)
    {
        while (true)
        {
            var toxin = Toxin.Constructor(this.toxin, new Vector3(0f, 10f, 6f));
            toxin.Go();

            yield return new WaitForSeconds(timeInterval);
        }
    }

    private void Start()
    {
        WorldComponents.MainPlane = this.mainPlane;
        WorldComponents.TowerPrefab = this.towerPrefab;
        WorldComponents.WallPrefab = this.wallPrefab;
        WorldComponents.SoldierPrefab = this.soldierPrefab;

        var tower0 = Suppressor.Constructor(new Vector3(-5f, 0f, 5f), new SimpleTowerBuilder(this.towerPrefab), 2f);
        var tower1 = Suppressor.Constructor(new Vector3(5f, 0f, -5f), new SimpleTowerBuilder(this.towerPrefab), 2f);
       

        var wall01 = Wall.Constructor(tower0, tower1, new SimpleWallBuilder(this.wallPrefab));

        var soldier = Soldier.Constructor(this.soldierPrefab, 9f, 5f, tower0);
        var patrol = soldier.SetPatrol(wall01);

        this.StartCoroutine(this.GenerateToxins(5f));
    }
}
