public class BuildingUIWallTowerSelected : IBuildingUIState
{
    public BuildingUIWallTowerSelected(BuildingUI buildingUi)
    {
        this.BuildingUi = buildingUi;
    }

    public BuildingUI BuildingUi { get; }

    public IBuildingUIState Update()
    {
        // TODO выбор второй башни
        return this;
    }

    public IBuildingUIState Cancel()
    {
        return new BuildingUIWallPlacing(this.BuildingUi);
    }
}
