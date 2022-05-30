using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTuto : MonoBehaviour
{
    [SerializeField] int checkPointIndex;
    [SerializeField] RobotTutoController robot;
    bool tutoHasLaunched;
    // Start is called before the first frame update
    void Start()
    {
        tutoHasLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7 && !tutoHasLaunched)
        {
            robot.LaunchTuto(checkPointIndex);
            tutoHasLaunched = true;
        }
    }
}
