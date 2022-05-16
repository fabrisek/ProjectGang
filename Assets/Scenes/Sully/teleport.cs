using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 destination;

    // Update is called once per frame
    void OnCollisionEnter(Collision col)
    {
        if(this.name=="portail1")
        {
            destination = GameObject.Find("portail2").transform.position;
        }
        else
        {
            destination = GameObject.Find("portail1").transform.position;
        }

        col.transform.position = destination - Vector3.forward * 3;
        col.transform.Rotate(Vector3.up * 180);

    }
}
