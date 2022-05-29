using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTutoController : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
    }
    void LookAtPlayer()
    {
        transform.forward = (player.transform.position - transform.position);
    }
    public void LaunchTuto(int checkPointId)
    {
        Debug.Log("LaunchTuto1");
    }
}
