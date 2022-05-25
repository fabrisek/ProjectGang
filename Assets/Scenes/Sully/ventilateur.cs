using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Ventilateur : MonoBehaviour
{
    public float force;
    public float amortissement;

    Vector3 dir = Vector3.up;

    [SerializeField] ShakeData ventiloShake;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   void OnTriggerStay(Collider other)
   {
       if (other.GetComponent<Rigidbody>() != null)
       {
         Rigidbody rb = other.GetComponent<Rigidbody>();
         Vector3 velocity = rb.velocity;
         velocity += dir * force * Time.deltaTime;
         velocity -= velocity *  amortissement * Time.deltaTime;
         rb.velocity = velocity;

         // rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
         //rb.velocity = (Vector3.Normalize(Vector3.up * force * rb.mass));
       }
   }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            CameraShakerHandler.Shake(ventiloShake);
        }
    }

}
