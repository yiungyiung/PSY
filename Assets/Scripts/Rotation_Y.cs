using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Y : MonoBehaviour
{
    public float rotationSpeed = 200f; 

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float rotationChange = mouseInputX * rotationSpeed * Time.deltaTime;
            transform.localRotation *= Quaternion.Euler(0, -rotationChange, 0);
        }
    }
}