using UnityEngine;
using UnityEngine.UI;

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
    public void TowerButton(Button sender)
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

        this.State = this.State.Cancel();
    }

    /// <summary>
    /// Когда нажали кнопку для строительства <see cref="Wall"/>.
    /// </summary>
    public void WallButton(Button sender)
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

        this.State = this.State.Cancel();
    }

    /// <summary>
    /// Когда нажали кнопку выставления солдата <see cref="Soldier"/>
    /// </summary>
    public void SoldierButton(Button sender)
    {
        if (this.State is BuildingUIFree)
        {
            this.State = new BuildingUISoldierPlacing(this);
            return;
        }
        else if (this.State is BuildingUISoldierPlacing)
        {
            var placing = this.State as BuildingUISoldierPlacing;
            this.State = placing.Cancel();
            return;
        }
        else if (this.State is BuildingUIPatrolPlacing)
        {
            this.State = this.State.Cancel();
            return;
        }

        this.State = this.State.Cancel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.State = this.State.Cancel();
            return;
        }

        this.State = this.State.Update();
    }
}
