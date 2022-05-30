using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTuto : MonoBehaviour
{
    [SerializeField] RobotTutoController robotTutoController;
    public int startIndex = 0;
    Transform currentCPPos;
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
        if(other.gameObject.layer == 15)
        {
            transform.position = robotTutoController.RespawnPosition(startIndex).position;
            
        }
    }
}
