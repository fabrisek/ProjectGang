using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Magnet : MonoBehaviour

{
    [SerializeField] float force;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (other.GetComponent<Rigidbody>())
                other.GetComponent<Rigidbody>().AddForce((transform.position - other.transform.position) * force * Time.smoothDeltaTime);
        }
    }

}