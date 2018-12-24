using System;

using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUIPatrolPlacing : IBuildingUIState
{
    private Soldier soldier;

    private Patrol patrol;

    public BuildingUIPatrolPlacing(Soldier soldier, BuildingUI buildingUi)
    {
        this.soldier = soldier;
        this.BuildingUi = buildingUi;
    }

    public BuildingUI BuildingUi { get; }

    public IBuildingUIState Cancel()
    {
        MonoBehaviour.Destroy(this.soldier.gameObject);
        if (this.patrol != null)
        {
            this.patrol.Destroy();
        }

        return new BuildingUIFree(this.BuildingUi);
    }

    public IBuildingUIState Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return this;
            }

            var layers = LayerMask.GetMask("Walls");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = default(RaycastHit);
            var isHit = Physics.Raycast(ray, out hit, float.MaxValue, layers);
            if (isHit)
            {
                Debug.Log("Hit!");
                var wall = hit.collider.gameObject.transform.parent.gameObject.GetComponent<Wall>();

                if (this.patrol != null)
                {
                    this.patrol.Add(wall);
                }
                else
                {
                    this.patrol = this.soldier.SetPatrol(wall);
                }

                Debug.Log("Patrol expanded!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            return new BuildingUIFree(this.BuildingUi);
        }

        return this;
    }
}