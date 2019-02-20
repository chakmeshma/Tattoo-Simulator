using UnityEngine;

public class TattooPositioning : MonoBehaviour
{
    public Transform model;
    public GameObject decal;
    public bool active;
    private ch.sycoforge.Decal.EasyDecal lastDecalCandidate = null;
    private ch.sycoforge.Decal.EasyDecal lastDecal = null;
    private ch.sycoforge.Decal.EasyDecal preservedDecal = null;
    private System.Collections.Generic.List<GameObject> disposables = new System.Collections.Generic.List<GameObject>();

    System.Collections.IEnumerator dispose()
    {
        while (true)
        {
            for (int i = 0; i < disposables.Count; ++i)
            {
                Destroy(disposables[i]);
            }
            disposables.Clear();


            yield return new WaitForSeconds(0.016f);
        }
    }

    private void Awake()
    {
        StopAllCoroutines();
        StartCoroutine(dispose());
    }

    private void Update()
    {
        if (lastDecal)
        {
            if (lastDecal == preservedDecal)
            {
                preservedDecal = null;
            }
            else
            {
                disposables.Add(lastDecal.gameObject);
                lastDecal.gameObject.SetActive(false);
            }
        }


        //if(Input.GetKeyDown(KeyCode.LeftControl))
        //{
        //    decal.SetActive(true);
        //}

#if UNITY_STANDALONE || UNITY_WEBGL
        if (active)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IPHONE
        if (active && Input.touchCount == 2)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[1].position);
#endif

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 10.0f))
            {
                lastDecalCandidate = ch.sycoforge.Decal.EasyDecal.ProjectAt(decal, hitInfo.collider.gameObject, hitInfo.point, hitInfo.normal);
                lastDecalCandidate.gameObject.SetActive(true);
                lastDecalCandidate.LateUnbake();
                //lastDecalCandidate.gameObject.SetActive(true);

#if UNITY_STANDALONE || UNITY_WEBGL
                if (Input.GetMouseButtonDown(0))
                {
#elif UNITY_ANDROID || UNITY_IPHONE
                if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                    Input.GetTouch(1).phase == TouchPhase.Ended)
                {
#endif
                    preservedDecal = lastDecalCandidate;
                    preservedDecal.Baked = false;
                    preservedDecal.LateBake();
                }
            }
        }

        //if(Input.GetKeyUp(KeyCode.LeftControl))
        //{
        //    decal.SetActive(false);
        //}
    }

    private void LateUpdate()
    {
        lastDecal = lastDecalCandidate;
    }
}
