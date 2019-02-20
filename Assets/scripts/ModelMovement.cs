using UnityEngine;

public class ModelMovement : MonoBehaviour
{
    public bool active = true;
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
    private float touchDistanceLast;

    public float zoomPositionOrigin { get; private set; }
#endif


    private void Awake()
    {
        resetOffsetValues();
    }

    public void resetOffsetValues()
    {
        startOffset = transform.position;

        zoomPosition = startOffset.z;
        position.x = startOffset.x;
        position.y = startOffset.y;
    }

    private void Update()
    {
        if (active)
        {
#if UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButton(1))
        {

            position += new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * panSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl))
        { 
              model.Rotate(new Vector3(0.0f, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);
        }

        float wheelDelta = Input.GetAxis("Mouse ScrollWheel");

        zoomPosition += wheelDelta * zoomSpeed * Time.deltaTime;
        zoomPosition = Mathf.Clamp(zoomPosition, startOffset.z + zoomBounds.x, startOffset.z + zoomBounds.y);

        transform.position = new Vector3(position.x, position.y, zoomPosition);
#elif UNITY_ANDROID

            if (Input.touchCount == 2 &&
                Input.GetTouch(0).phase != TouchPhase.Ended &&
                Input.GetTouch(0).phase != TouchPhase.Canceled &&
                Input.GetTouch(1).phase != TouchPhase.Ended &&
                Input.GetTouch(1).phase != TouchPhase.Canceled)
            {
                Vector2 touchMean = (Input.touches[0].position + Input.touches[1].position) / 2.0f;
                float touchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);


                if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
                {
                    touchOrigin = touchMean;
                    positionOrigin = position;
                    touchDistanceLast = touchDistance;
                    zoomPositionOrigin = zoomPosition;
                }

                Vector2 touchOffset = (touchMean - touchOrigin) / 100.0f * panSpeed;

                position = positionOrigin - touchOffset;

                zoomPosition = zoomPosition + ((touchDistance - touchDistanceLast) / 100.0f);
                zoomPosition = Mathf.Clamp(zoomPosition, startOffset.z + zoomBounds.x, startOffset.z + zoomBounds.y);

                touchDistanceLast = touchDistance;



                transform.position = new Vector3(position.x, position.y, zoomPosition);
            }
            else if (Input.touchCount == 1 &&
                Input.GetTouch(0).phase != TouchPhase.Ended &&
                Input.GetTouch(0).phase != TouchPhase.Canceled)
            {
                Vector2 touchDelta;

                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    touchDelta = Vector2.zero;
                }
                else
                {
                    touchDelta = Input.touches[0].deltaPosition;
                }

                model.Rotate(new Vector3(0.0f, -touchDelta.x, 0) * rotationSpeed);
            }

#endif
        }
    }
}
