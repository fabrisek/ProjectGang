using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform cameraPosition;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraPosition.position, 20*Time.deltaTime);
    }
}
