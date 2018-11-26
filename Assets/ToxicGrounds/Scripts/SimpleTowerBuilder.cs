using UnityEngine;

public class SimpleTowerBuilder : ITowerBuilder
{
    private readonly GameObject simpleTowerPrefab;

    private GameObject body;

    public SimpleTowerBuilder(GameObject simpleTowerPrefab)
    {
        this.simpleTowerPrefab = simpleTowerPrefab;
    }

    public void Build(Suppressor suppressor)
    {
        this.body = UnityEngine.Object.Instantiate(this.simpleTowerPrefab, suppressor.transform);
    }

    public void Destroy()
    {
        UnityEngine.Object.Destroy(this.body);
    }
}
