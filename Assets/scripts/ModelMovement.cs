using UnityEngine;

public class ModelMovement : MonoBehaviour
{
    public Transform model;
    public float panSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public Vector2 zoomBounds;
    private float zoomPosition;
    private Vector2 position;
    private Vector3 startOffset;
#if UNITY_ANDROID || UNITY_IPHONE
    private Vector2 touchOrigin;
    private Vector2 positionOrigin;
#endif


    private void Awake()
    {
        startOffset = transform.position;

        zoomPosition = startOffset.z;
        position.x = startOffset.x;
        position.y = startOffset.y;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButton(1))
        {

            panPosition += new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * panSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl))
        { 
              model.Rotate(new Vector3(0.0f, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);
        }

        float wheelDelta = Input.GetAxis("Mouse ScrollWheel");

        zoomPosition += wheelDelta * zoomSpeed * Time.deltaTime;
        zoomPosition = Mathf.Clamp(zoomPosition, startOffset.z + zoomBounds.x, startOffset.z + zoomBounds.y);

        transform.position = new Vector3(panPosition.x, panPosition.y, zoomPosition);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 2)
            {
                Vector2 touchMean = (Input.touches[0].position + Input.touches[1].position) / 2.0f;

                if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
                {
                    touchOrigin = touchMean;
                    positionOrigin = position;
                }

                Vector2 touchOffset = (touchMean - touchOrigin) / 100.0f;

                position = positionOrigin - touchOffset;

                float wheelDelta = Input.GetAxis("Mouse ScrollWheel");

                zoomPosition += wheelDelta * zoomSpeed * Time.deltaTime;
                zoomPosition = Mathf.Clamp(zoomPosition, startOffset.z + zoomBounds.x, startOffset.z + zoomBounds.y);

                transform.position = new Vector3(position.x, position.y, zoomPosition);
            }

            if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl))
            {
                model.Rotate(new Vector3(0.0f, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);
            }
        }
#endif
    }
}
