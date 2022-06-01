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
        Debug.Log(other.transform.name + " " + other.transform.gameObject.layer + " " + (other.gameObject.layer == 7));
        Debug.Log(other.transform.gameObject.layer);
        if (other.gameObject.layer == 7)
        {
            Debug.Log(other.transform.name + " " + "On me voie");
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
