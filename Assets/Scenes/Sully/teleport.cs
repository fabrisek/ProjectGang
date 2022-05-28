using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform destination;

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<Rigidbody>()!= null)
        {
            col.transform.position = destination.position;
        }
    }
}
