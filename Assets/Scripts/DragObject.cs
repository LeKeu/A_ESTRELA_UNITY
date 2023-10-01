using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    Plane movePlane;
    float fixedDistance = 2f;
    float hitDist, t;
    Ray camRay;
    Vector3 startPos, point, corPoint;

    [SerializeField] Grid grid;

    public bool isMoving = false;

    void OnMouseUp()
    {
        grid.DesenharGrid();
    }

    void OnMouseDown()
    {
        isMoving = false;

        startPos = transform.position; // save position in case draged to invalid place
        movePlane = new Plane(-Camera.main.transform.forward, transform.position); // find a parallel plane to the camera based on obj start pos;
    }

    void OnMouseDrag()
    {
        
        camRay = Camera.main.ScreenPointToRay(Input.mousePosition); // shoot a ray at the obj from mouse screen point

        if (movePlane.Raycast(camRay, out hitDist))
        { // finde the collision on movePlane
            point = camRay.GetPoint(hitDist); // define the point on movePlane
            t = -(fixedDistance - camRay.origin.y) / (camRay.origin.y - point.y); // the x,y or z plane you want to be fixed to
            corPoint.x = camRay.origin.x + (point.x - camRay.origin.x) * t; // calculate the new point t futher along the ray
            corPoint.y = 0;
            corPoint.z = camRay.origin.z + (point.z - camRay.origin.z) * t;
            transform.position = corPoint;
        }
        isMoving = true;
    }
}
