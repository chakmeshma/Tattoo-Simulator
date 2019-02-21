using UnityEngine;

public class ModelMovement : MonoBehaviour
{
    public bool active = true;
    public GameObject model;
    public Vector2 zoomBounds;
    private float zoomPosition;
    private Vector2 position;
    private Vector3 startOffset;
#if UNITY_ANDROID || UNITY_IPHONE
    private Vector2 touchOrigin;
    private Vector2 positionOrigin;
    private float touchDistanceLast;
#endif
    [Header("Standalone & PC")]
    public float SW_panSpeed;
    public float SW_rotationSpeed;
    public float SW_zoomSpeed;
    [Header("Handheld")]
    public float H_panSpeed;
    public float H_rotationSpeed;
    public float H_zoomSpeed;

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

                position += new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * SW_panSpeed * Time.deltaTime;
            }

            if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl))
            {
                model.transform.Rotate(new Vector3(0.0f, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * SW_rotationSpeed);
            }

            float wheelDelta = Input.GetAxis("Mouse ScrollWheel");


#if UNITY_STANDALONE || UNITY_WEBGL
            zoomPosition += wheelDelta * SW_zoomSpeed * Time.deltaTime;
#elif UNITY_ANDROID || UNITY_IPHONE
            zoomPosition += wheelDelta * zoomSpeed * Time.deltaTime;
#endif

            zoomPosition = Mathf.Clamp(zoomPosition, startOffset.z + zoomBounds.x, startOffset.z + zoomBounds.y);

            transform.position = new Vector3(position.x, position.y, zoomPosition);
#elif UNITY_ANDROID || UNITY_IPHONE

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
                }

#if UNITY_STANDALONE || UNITY_WEBGL
                Vector2 touchOffset = (touchMean - touchOrigin) / 100.0f * SW_panSpeed;
#elif UNITY_ANDROID || UNITY_IPHONE
                Vector2 touchOffset = (touchMean - touchOrigin) / 100.0f * H_panSpeed;
#endif

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

#if UNITY_STANDALONE || UNITY_WEBGL
                model.transform.Rotate(new Vector3(0.0f, -touchDelta.x, 0) * SW_rotationSpeed);
#elif UNITY_ANDROID || UNITY_IPHONE
                model.transform.Rotate(new Vector3(0.0f, -touchDelta.x, 0) * H_rotationSpeed);
#endif
            }

#endif
        }
    }
}
