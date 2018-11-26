using UnityEngine;

public class TestBehaviour : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Started");
    }

    private void Awake()
    {
        Debug.Log("Awaken");
    }

    private void OnEnable()
    {
        Debug.Log("Enabled");
    }
}
