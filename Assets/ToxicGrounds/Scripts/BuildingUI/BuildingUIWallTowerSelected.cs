using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUIWallTowerSelected : IBuildingUIState
{
    private Suppressor firstTower;

    public BuildingUIWallTowerSelected(BuildingUI buildingUi, Suppressor tower)
    {
        this.BuildingUi = buildingUi;
        this.firstTower = tower;
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

            var layers = LayerMask.GetMask("TowersOnly");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = default(RaycastHit);
            var isHit = Physics.Raycast(ray, out hit, layers);
            if (isHit)
            {
                var newWall = Wall.Constructor(
                    this.firstTower,
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<Suppressor>(),
                    new SimpleWallBuilder(WorldComponents.WallPrefab));

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
        return new BuildingUIWallPlacing(this.BuildingUi);
    }
}
