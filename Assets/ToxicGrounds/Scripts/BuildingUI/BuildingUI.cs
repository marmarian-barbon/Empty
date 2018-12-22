using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public GameObject CanvasPrefab { get; private set; }

    public IBuildingUIState State { get; private set; }

    public static BuildingUI Constructor(GameObject canvasPrefab)
    {
        var result = MonoBehaviour.Instantiate(canvasPrefab).AddComponent<BuildingUI>();
        result.CanvasPrefab = canvasPrefab;
        result.State = new BuildingUIFree(result);
        return result;
    }

    /// <summary>
    /// Когда нажали кнопку для строительства <see cref="Suppressor"/>.
    /// </summary>
    public void TowerButton()
    {
        switch (this.State)
        {
            case BuildingUIFree _:
                this.State = new BuildingUITowerPlacing(this);
                return;
            case BuildingUITowerPlacing placing:
                this.State = placing.Cancel();
                return;
        }
    }

    /// <summary>
    /// Когда нажали кнопку для строительства <see cref="Wall"/>.
    /// </summary>
    public void WallButton()
    {
        switch (this.State)
        {
            case BuildingUIFree _:
                this.State = new BuildingUIWallPlacing(this);
                return;
            case BuildingUIWallPlacing placing:
                this.State = placing.Cancel();
                return;
            case BuildingUIWallTowerSelected towerSelected:
                this.State = towerSelected.Cancel().Cancel();
                return;
        }
    }

    /// <summary>
    /// Когда нажали кнопку выставления солдата <see cref="Soldier"/>
    /// </summary>
    public void SoldierButton()
    {
        // TODO
    }

    private void Update()
    {
        this.State = this.State.Update();
    }
}
