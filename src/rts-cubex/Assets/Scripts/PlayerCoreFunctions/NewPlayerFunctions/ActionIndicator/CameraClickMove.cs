using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClickMove : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] Camera cameraScanner;
    [SerializeField] float mapSize;
    Ray ray;
    RaycastHit hit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;
        ray = cameraScanner.ScreenPointToRay(mousePosition * mapSize);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            print("Found object through minimap called: " + hit.collider.tag + ", " + hit.point);
        }
    }
}
