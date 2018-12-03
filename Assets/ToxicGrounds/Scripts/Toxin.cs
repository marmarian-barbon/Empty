using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Toxin : MonoBehaviour
{
    private IEnumerator currentCoroutine;

    public IEnumerator CurrentCoroutine
    {
        get
        {
            return this.currentCoroutine;
        }

        set
        {
            this.currentCoroutine = value;
            this.StartCoroutine(this.currentCoroutine);
        }
    }

    public float Size { get; private set; }

    public static Toxin Constructor(GameObject prefab, Vector3 position)
    {
        var result = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity).AddComponent<Toxin>();
        result.Size = result.gameObject.GetComponent<SphereCollider>().radius;
        return result;
    }
}
