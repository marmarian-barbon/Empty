using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;

public class Toxin : MonoBehaviour
{
    public ICollection<Watch> TriggeredBy { get; private set; } = new List<Watch>();

    private Vector3 endPoint = new Vector3(35f, 0.5f, 44f);

    private Vector3 firstPoint = new Vector3(-30f, 0.5f, -30f);

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

    private double health = 100;

    public double Health
    {
        get
        {
            return this.health;
        }
        set
        {
            this.health = value;
            if (this.health <= 0)
            {
                Debug.Log("Dead");
                MonoBehaviour.DestroyImmediate(this.gameObject, allowDestroyingAssets: true);
                foreach (var watch in this.TriggeredBy)
                {
                    watch.CheckTarget(this);
                }
            }
        }
    }

    public float Size { get; private set; }

    public void Go()
    {
        var maxMove = Vector3.Distance(this.firstPoint, this.endPoint) / 3;
        this.gameObject.transform.position = this.firstPoint;
        var currentPosition = this.gameObject.transform.position;
        var moves = new List<IEnumerator>();

        while (Vector3.Distance(currentPosition, this.endPoint) > Vector3.kEpsilon)
        {
            var rest = this.endPoint - currentPosition;
            var addPosition = Quaternion.Euler(0f, Random.Range(-45f, 45f), 0f) * (rest.normalized * maxMove);
            var angle = Vector3.Angle(-rest, -rest + addPosition);
            var nextPosition = currentPosition + addPosition;
            if (angle > 90f)
            {
                nextPosition = this.endPoint;
            }

            moves.Add(this.Move(nextPosition));
            currentPosition = nextPosition;
        }

        this.CurrentCoroutine = this.FullMove(moves);

    }

    private IEnumerator FullMove(IList<IEnumerator> moves)
    {
        foreach (var move in moves)
        {
            yield return this.StartCoroutine(move);
        }
    }

    private IEnumerator Move(Vector3 to)
    {
        do
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, to, Time.deltaTime * 3f);
            yield return new WaitForEndOfFrame();
        }
        while (Vector3.Distance(this.transform.position, to) > Vector3.kEpsilon);
    }

    public static Toxin Constructor(GameObject prefab, Vector3 position)
    {
        var result = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity).AddComponent<Toxin>();
        result.Size = result.gameObject.GetComponent<SphereCollider>().radius;
        return result;
    }
}
