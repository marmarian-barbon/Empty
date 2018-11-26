using System.Collections.Generic;

using UnityEngine;

public class SimpleWallBuilder : IWallBuilder
{
    private readonly GameObject wallPrefab;

    private GameObject body;

    public SimpleWallBuilder(GameObject wallPrefab)
    {
        this.wallPrefab = wallPrefab;
    }

    public void Build(Wall wall)
    {
        this.body = MonoBehaviour.Instantiate(this.wallPrefab, wall.transform);
    }

    public void Destroy()
    {
        MonoBehaviour.Destroy(this.body);
    }
}
