﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;
    public float minZoomDistance;
    public float maxZoomDistance;
    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float topLimit;
    [SerializeField] float botLimit;
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Move();
        Zoom();
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        Vector3 dir = transform.forward * zInput + transform.right * xInput;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
        transform.position.y,
        Mathf.Clamp(transform.position.z, botLimit, topLimit)
        );
    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDistance && scrollInput > 0.0f)
            return;
        else if (dist > maxZoomDistance && scrollInput < 0.0f)
            return;
        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }

    public void FocusOnPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}