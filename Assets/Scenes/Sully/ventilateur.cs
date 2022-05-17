using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilateur : MonoBehaviour
{
    public float force;
    public float _force;

    Vector3 dir = Vector3.up;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        

         void OnTriggerStay(Collider other)
         {
            Debug.Log("collizion");
            if (other.GetComponent<Rigidbody>() != null)
            {
                Debug.Log("gotRigidBody");
                Rigidbody rb = other.GetComponent<Rigidbody>();
                Vector3 velocity = rb.velocity;
                velocity += dir * force * Time.deltaTime;
                velocity -= velocity * _force * Time.deltaTime;
                rb.velocity = velocity;

                // rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
                //rb.velocity = (Vector3.Normalize(Vector3.up * force * rb.mass));
            }
         }
    }

    
}
