using UnityEngine;

public static class WorldComponents
{
    public static GameObject MainPlane { get; set; }

    public static string WallPrefabName { get; } = "SimpleWallPrefab";

    public static string TowerPrefabName { get; } = "SimpleTowerPrefab";

    public static string UiBuildingCanvasPrefabName { get; } = "UIBuildingCanvas";

    public static GameObject TowerPrefab;

    public static GameObject WallPrefab;

    public static GameObject SoldierPrefab;
}
