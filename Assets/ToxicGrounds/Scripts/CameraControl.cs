using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float CameraAngleSpeed { get; private set; }

    public float CameraDirectionSpeed { get; private set; }

    public float MaxLength { get; private set; }

    public float MinLength { get; private set; }

    // Use this for initialization
    private void Start()
    {
        this.CameraDirectionSpeed = 0.1f;
        this.CameraAngleSpeed = 1f;
        this.MinLength = 5f;
        this.MaxLength = 40f;

        this.gameObject.transform.LookAt(Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            var cameraTransform = this.gameObject.transform;

            if (cameraTransform.position.magnitude > this.MinLength)
            {
                cameraTransform.position += cameraTransform.forward * 0.1f;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            var cameraTransform = this.gameObject.transform;

            if (cameraTransform.position.magnitude < this.MaxLength)
            {
                cameraTransform.position -= cameraTransform.forward * 0.1f;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            var cameraTransform = this.gameObject.transform;

            cameraTransform.RotateAround(Vector3.zero, Vector3.up, this.CameraAngleSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            var cameraTransform = this.gameObject.transform;

            cameraTransform.RotateAround(Vector3.zero, Vector3.up, -this.CameraAngleSpeed);
        }
    }
}