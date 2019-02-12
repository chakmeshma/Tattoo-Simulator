using UnityEngine;

public class ModelMovement : MonoBehaviour
{
    public Transform model;
    public float panSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public Vector2 zoomBounds;
    private float zoomPosition;
    private Vector2 panPosition;
    private Vector3 startOffset;

    private void Awake()
    {
        startOffset = transform.position;

        zoomPosition = startOffset.z;
        panPosition.x = startOffset.x;
        panPosition.y = startOffset.y;

    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            model.Rotate(new Vector3(0.0f, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);
        }

        if (Input.GetMouseButton(0))
        {
            panPosition += new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * panSpeed * Time.deltaTime;

            //transform.position = new Vector3(panPosition.x , panPosition.y, transform.position.z);
        }

        float wheelDelta = Input.GetAxis("Mouse ScrollWheel");

        zoomPosition += wheelDelta * zoomSpeed * Time.deltaTime;
        zoomPosition = Mathf.Clamp(zoomPosition, startOffset.z + zoomBounds.x, startOffset.z + zoomBounds.y);

        transform.position = new Vector3(panPosition.x, panPosition.y, zoomPosition);
    }
}
