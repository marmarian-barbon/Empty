public class BuildingUIWallPlacing : IBuildingUIState
{
    public BuildingUIWallPlacing(BuildingUI buildingUi)
    {
        this.BuildingUi = buildingUi;
    }

    public BuildingUI BuildingUi { get; }

    public IBuildingUIState Update()
    {
        // TODO выбор первой башни
        return this;
    }

    public IBuildingUIState Cancel()
    {
        return new BuildingUIFree(this.BuildingUi);
    }
}
