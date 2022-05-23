using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovCamera : MonoBehaviour
{
    [SerializeField] Camera camera;
    Rigidbody rb;
    [SerializeField] [Range(0,10)] float fovEffect;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        camera.fieldOfView = 90 + Mathf.Sqrt(rb.velocity.magnitude)*fovEffect;
    }
}
