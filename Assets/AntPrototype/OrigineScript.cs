using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrigineScript : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer = default;
    [SerializeField] float stepHeight = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGrounded())
        {
            Vector3 checkNextPoint = new Vector3(transform.position.x, transform.position.y + (stepHeight - 0.1f), transform.position.z);
            RaycastHit[] hits = Physics.RaycastAll(checkNextPoint, Vector3.down, stepHeight, groundLayer);
            if (hits.Length >= 1)
            {
                gameObject.transform.position = new Vector3(transform.position.x, hits[0].transform.position.y, transform.position.z);
            }
            else
            {
                hits = Physics.RaycastAll(transform.position, Vector3.down, stepHeight, groundLayer);
                if (hits.Length >= 1)
                {
                    gameObject.transform.position = new Vector3(transform.position.x, hits[0].transform.position.y, transform.position.z);
                }
            }
        }
    }

    bool IsGrounded()
    {

        //   Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y -0.1f, transform.position.z), Vector3.down);
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), Vector3.down, 0.1f, groundLayer);
        if (hits.Length >= 1)
        {
            return true;
        }

            return false;
    }
}
