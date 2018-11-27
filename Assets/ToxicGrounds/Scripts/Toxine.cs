using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Toxine : MonoBehaviour
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

    public static Toxine Constructor(GameObject prefab, Vector3 position)
    {
        var result = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity).AddComponent<Toxine>();
        return result;
    }
}
