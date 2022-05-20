using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFollow : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.gameManager.Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == playerLayer)
        {
            Debug.Log(other.gameObject);
            player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            player.transform.parent = transform;
        }
    }
}
