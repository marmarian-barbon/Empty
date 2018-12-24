using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUISoldierPlacing : IBuildingUIState
{
    public BuildingUISoldierPlacing(BuildingUI buildingUi)
    {
        this.BuildingUi = buildingUi;
    }

    public BuildingUI BuildingUi { get; }

    public IBuildingUIState Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return this;
            }

            Debug.Log("SoldierPlacing 0!");
            var layers = LayerMask.GetMask("Towers");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = default(RaycastHit);
            var isHit = Physics.Raycast(ray, out hit, float.MaxValue, layers);
            if (isHit)
            {
                Debug.Log("SoldierPlacing 1!");
                var soldier = Soldier.Constructor(
                    WorldComponents.SoldierPrefab,
                    9f,
                    10f,
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<Suppressor>());

                return new BuildingUIPatrolPlacing(soldier, this.BuildingUi);
            }
            else
            {
                return this;
            }
        }

        return this;
    }

    public IBuildingUIState Cancel()
    {
        return new BuildingUIFree(this.BuildingUi);
    }
}

