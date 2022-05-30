using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTuto : MonoBehaviour
{
    [SerializeField] int checkPointIndex;
    [SerializeField] int tutoIndexToLaunch;
    [SerializeField] RobotTutoController robot;
    bool tutoHasLaunched;
    [SerializeField] DeathTuto deathTuto;
    [SerializeField] GameObject ArrowEffect;
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
            robot.LaunchTuto(tutoIndexToLaunch);
            tutoHasLaunched = true;
            deathTuto.startIndex = checkPointIndex;
            ArrowEffect.SetActive(false);
            AudioManager.instance.playSoundEffect(9, 0.3f);
        }
    }
}
