using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_X : MonoBehaviour
{
    public float rotationSpeed = 200f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseInputY = Input.GetAxis("Mouse Y");
            float rotationChange = mouseInputY * rotationSpeed * Time.deltaTime;
            transform.localRotation *= Quaternion.Euler(rotationChange, 0, 0);
        }
    }
}
