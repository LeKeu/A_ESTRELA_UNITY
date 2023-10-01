using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObstacle : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    public bool obsCreated = false;
    [SerializeField] Grid grid;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                if(raycastHit.collider.gameObject.tag == "Obstacle")
                {
                    Destroy(raycastHit.collider.gameObject);
                    grid.DesenharGrid();

                }
                else
                {
                    Instantiate(prefab, raycastHit.point, Quaternion.identity);
                    grid.DesenharGrid();

                }
                
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            grid.DesenharGrid();
        }
    }

    
}
