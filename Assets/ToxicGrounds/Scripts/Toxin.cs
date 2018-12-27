using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Toxin : MonoBehaviour
{
    private readonly Vector3 endPoint = new Vector3(35f, 0.5f, 44f);

    private readonly Vector3 firstPoint = new Vector3(-30f, 0.5f, -30f);

    private readonly ISet<Watch> triggeredBy = new HashSet<Watch>();

    private IEnumerator currentRoutine;

    private double health = 100;

    public IEnumerator CurrentRoutine
    {
        get
        {
            return this.currentRoutine;
        }

        set
        {
            this.currentRoutine = value;
            this.StartCoroutine(this.currentRoutine);
        }
    }

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
                this.gameObject.SetActive(false);
                MonoBehaviour.Destroy(this.gameObject);
                foreach (var watch in this.triggeredBy)
                {
                    watch.Check(this);
                }
            }
        }
    }

    public float Size { get; private set; }

    public static Toxin Constructor(GameObject prefab, Vector3 position)
    {
        var result = Instantiate(prefab, position, Quaternion.identity).AddComponent<Toxin>();
        result.Size = result.gameObject.GetComponent<SphereCollider>().radius;
        return result;
    }

    public void Triggered(Watch watch)
    {
        this.triggeredBy.Add(watch);
    }

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

        this.CurrentRoutine = this.FullMove(moves);
    }

    private IEnumerator FullMove(IEnumerable<IEnumerator> moves)
    {
        return moves.Select(this.StartCoroutine).GetEnumerator();
    }

    private IEnumerator Move(Vector3 to)
    {
        var newRotation = this.transform.rotation;
        newRotation.SetLookRotation(to - this.transform.position, Vector3.up);
        this.transform.rotation = newRotation;
        do
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, to, Time.deltaTime * 3f);
            yield return new WaitForEndOfFrame();
        }
        while (Vector3.Distance(this.transform.position, to) > Vector3.kEpsilon);
    }
}