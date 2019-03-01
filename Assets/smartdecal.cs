using System.Collections;
using UnityEngine;

public class smartdecal : MonoBehaviour
{
    public Object theObject;
    private float radius;

    private void falldown(int x, int z, ref Vector3[] newVertices, Vector3 lastPoint, bool horizontal, bool backward)
    {
        RaycastHit hitInfo = new RaycastHit();
        float degreeStep = 10.0f;
        int index;

        for (int i = 0; i < 5; ++i)
        {
            for (float degree = 0.0f; degree < 360.0f; degree += degreeStep)
            {
                Vector3 circlePointVector = (radius * Mathf.Cos(degree * Mathf.Deg2Rad) * ((horizontal) ? (transform.right) : (transform.forward)))
                    + (radius * Mathf.Sin(degree * Mathf.Deg2Rad) * transform.up);

                if (backward)
                {
                    circlePointVector *= -1;
                }

                Vector3 circlePoint = (horizontal) ? (lastPoint + circlePointVector) : (lastPoint - circlePointVector);

                Vector3 rayVector;

                if (backward)
                {
                    rayVector = -Vector3.Cross(circlePointVector, (horizontal) ? (transform.forward) : (transform.right));
                }
                else
                {
                    rayVector = Vector3.Cross(circlePointVector, (horizontal) ? (transform.forward) : (transform.right));
                }

                float rayLength = (degreeStep / 360.0f) * 2.0f * Mathf.PI * radius;

                if (Physics.Raycast(circlePoint, rayVector, out hitInfo, rayLength))
                {
                    if (backward)
                    {
                        index = getVertexIndexAt((horizontal) ? (x - 1) : (x), (horizontal) ? (z) : (z - 1));
                    }
                    else
                    {
                        index = getVertexIndexAt((horizontal) ? (x + 1) : (x), (horizontal) ? (z) : (z + 1));
                    }

                    newVertices[index] = transform.InverseTransformPoint(hitInfo.point);

                    break;
                }
            }

            lastPoint = hitInfo.point;
            if (horizontal)
            {
                if (backward)
                {
                    x--;
                }
                else
                {
                    x++;
                }
            }
            else
            {
                if (backward)
                {
                    z--;
                }
                else
                {
                    z++;
                }
            }
        }
    }

    private void vorgang(int x, int z)
    {
        RaycastHit hitInfo;


        if (raycastPerpendicularAt(x, z, out hitInfo))
        {
            int index = getVertexIndexAt(x, z);

            Vector3[] theVerties = GetComponent<MeshFilter>().mesh.vertices;
            Vector3[] newVertices = new Vector3[theVerties.Length];

            System.Array.Copy(theVerties, newVertices, theVerties.Length);

            newVertices[index] = transform.InverseTransformPoint(hitInfo.point);

            Vector3 lastPoint = hitInfo.point;

            falldown(x, z, ref newVertices, lastPoint, false, false);
            falldown(x, z, ref newVertices, lastPoint, true, false);
            falldown(x, z, ref newVertices, lastPoint, false, true);
            falldown(x, z, ref newVertices, lastPoint, true, true);
            //falldown(x, z, ref newVertices, lastPoint, false, false);

            for (int i = 1; i < 6; ++i)
            {
                lastPoint = transform.TransformPoint(newVertices[getVertexIndexAt(0, i)]);
                falldown(x, z + i, ref newVertices, lastPoint, true, false);
                falldown(x, z + i, ref newVertices, lastPoint, true, true);
                lastPoint = transform.TransformPoint(newVertices[getVertexIndexAt(0, -i)]);
                falldown(x, z - i, ref newVertices, lastPoint, true, false);
                falldown(x, z - i, ref newVertices, lastPoint, true, true);
            }

            for (int i = 1; i < 6; ++i)
            {
                lastPoint = transform.TransformPoint(newVertices[getVertexIndexAt(i, 0)]);
                falldown(x + i, z, ref newVertices, lastPoint, false, false);
                falldown(x + i, z, ref newVertices, lastPoint, false, true);
                lastPoint = transform.TransformPoint(newVertices[getVertexIndexAt(-i, 0)]);
                falldown(x - i, z, ref newVertices, lastPoint, false, false);
                falldown(x - i, z, ref newVertices, lastPoint, false, true);
            }

            //for (int i = 1; i < 6; ++i)
            //{
            //    lastPoint = transform.TransformPoint(newVertices[getVertexIndexAt(i, 0)]);
            //    falldown(x + i, z, ref newVertices, lastPoint, false, false);
            //    falldown(x - i, z, ref newVertices, lastPoint, false, false);
            //}

            GetComponent<MeshFilter>().mesh.vertices = newVertices;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            vorgang(0, 0);
        }

    }


    private void Start()
    {
        radius = (GetComponent<MeshFilter>().mesh.vertices[1] - GetComponent<MeshFilter>().mesh.vertices[0]).magnitude;

        //StartCoroutine(markVertex());

        //Instantiate(theObject, GetComponent<MeshFilter>().mesh.vertices[getVertexIndexAt(0, 0)], Quaternion.identity, transform);
        //Instantiate(theObject, GetComponent<MeshFilter>().mesh.vertices[getVertexIndexAt(0, 5)], Quaternion.identity, transform);
    }

    static int getVertexIndexAt(int x, int z)
    {
        int meshIndex = ((5 - z) * 11) + (5 - x);

        return meshIndex;
    }

    bool raycastPerpendicularAt(int x, int z, out RaycastHit hitInfo)
    {
        Vector3 origin = transform.TransformPoint(GetComponent<MeshFilter>().mesh.vertices[getVertexIndexAt(x, z)]);

        return Physics.Raycast(origin, -transform.up, out hitInfo, 20.0f);

        //Debug.DrawRay(origin, -transform.up, Color.blue, 20.0f);
    }

    IEnumerator markVertex()
    {
        int counter = 0;


        while (true)
        {
            Vector3 vertex = GetComponent<MeshFilter>().mesh.vertices[counter];

            Instantiate(theObject, vertex, Quaternion.identity, transform);

            counter++;

            yield return new WaitForSeconds(1.0f);
        }
    }
}
