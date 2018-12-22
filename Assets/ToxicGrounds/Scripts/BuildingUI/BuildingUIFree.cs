public class BuildingUIFree : IBuildingUIState
{
    public BuildingUIFree(BuildingUI buildingUi)
    {
        this.BuildingUi = buildingUi;
    }

    public BuildingUI BuildingUi { get; }

    public IBuildingUIState Update()
    {
        return this;
    }

    public IBuildingUIState Cancel()
    {
        return this;
    }
}
