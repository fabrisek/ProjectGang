using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerThePlayerBug : MonoBehaviour
{
    [SerializeField] GameObject viewBug;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            viewBug.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == 7)
        {
            viewBug.SetActive(false);
        }

    }
}
