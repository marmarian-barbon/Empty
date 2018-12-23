using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public IBuildingUIState State { get; private set; } = new BuildingUIFree(null);

    public static BuildingUI Constructor(GameObject canvasPrefab)
    {
        var result = MonoBehaviour.Instantiate(canvasPrefab).AddComponent<BuildingUI>();

        result.State = new BuildingUIFree(result);
        result.GetComponent<Canvas>().worldCamera = Camera.main;
        return result;
    }

    /// <summary>
    /// Когда нажали кнопку для строительства <see cref="Suppressor"/>.
    /// </summary>
    public void TowerButton()
    {
        if (this.State is BuildingUIFree)
        {
            Debug.Log("Place Tower");
            this.State = new BuildingUITowerPlacing(this);
            return;
        }
        else if (this.State is BuildingUITowerPlacing)
        {
            Debug.Log("Do not Place Tower");
            var placing = this.State as BuildingUITowerPlacing;
            this.State = placing.Cancel();
            return;
        }
    }

    /// <summary>
    /// Когда нажали кнопку для строительства <see cref="Wall"/>.
    /// </summary>
    public void WallButton()
    {
        if (this.State is BuildingUIFree)
        {
            this.State = new BuildingUIWallPlacing(this);
            return;
        }
        else if (this.State is BuildingUIWallPlacing)
        {
            var placing = this.State as BuildingUIWallPlacing;
            this.State = placing.Cancel();
            return;
        }
        else if (this.State is BuildingUIWallTowerSelected)
        {
            var towerSelected = this.State as BuildingUIWallTowerSelected;
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
