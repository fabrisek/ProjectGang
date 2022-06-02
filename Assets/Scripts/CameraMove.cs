using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform cameraPosition;

    void FixedUpdate()
    {
        gameObject.transform.position = cameraPosition.position;
    }
}
