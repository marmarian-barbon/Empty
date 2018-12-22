public class BuildingUITowerPlacing : IBuildingUIState
{
    public BuildingUITowerPlacing(BuildingUI buildingUi)
    {
        this.BuildingUi = buildingUi;
    }

    public BuildingUI BuildingUi { get; }

    public IBuildingUIState Update()
    {
        // TODO выбор места строительства башни
        return this;
    }

    public IBuildingUIState Cancel()
    {
        return new BuildingUIFree(this.BuildingUi);
    }
}
