using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUITowerPlacing : IBuildingUIState
{
    public BuildingUITowerPlacing(BuildingUI buildingUi)
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

            var layers = LayerMask.GetMask("Floor");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = default(RaycastHit);
            var isHit = Physics.Raycast(ray, out hit, float.MaxValue, layers);
            if (isHit)
            {
                Debug.Log("Hit!");
                var newTower = Suppressor.Constructor(
                    hit.point,
                    new SimpleTowerBuilder(WorldComponents.TowerPrefab),
                    2f);

                return new BuildingUIFree(this.BuildingUi);
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
