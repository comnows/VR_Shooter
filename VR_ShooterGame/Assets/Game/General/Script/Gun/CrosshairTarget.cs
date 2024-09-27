using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaySetup();

        if(Physics.Raycast(ray, out hitInfo))
        {
            SetHitPosition(hitInfo.point);
        }
        else
        {
            SetHitPosition(ray.GetPoint(100));
        }
    }

    private void RaySetup()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
    }

    private void SetHitPosition(Vector3 point)
    {
        transform.position = point;
    }
}
