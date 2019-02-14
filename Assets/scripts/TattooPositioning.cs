using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TattooPositioning : MonoBehaviour
{
    public Transform model;
    public GameObject decal;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            decal.SetActive(true);
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction);
            //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);


            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 10.0f)) {
                //Quaternion rotation = Quaternion.FromToRotation(-decal.transform.up, );

                decal.transform.up = hitInfo.normal;

                decal.transform.position = hitInfo.point;

                if(Input.GetMouseButtonDown(0))
                {
                    Instantiate(decal,decal.transform.parent, true);
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            decal.SetActive(false);
        }
    }
}
