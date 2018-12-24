using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUIWallPlacing : IBuildingUIState
{
    public BuildingUIWallPlacing(BuildingUI buildingUi)
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

            var layers = LayerMask.GetMask("Towers");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = default(RaycastHit);
            var isHit = Physics.Raycast(ray, out hit, float.MaxValue, layers);
            if (isHit)
            {
                var tower = hit.collider.gameObject.transform.parent.gameObject.GetComponent<Suppressor>();
                return new BuildingUIWallTowerSelected(this.BuildingUi, tower);
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
