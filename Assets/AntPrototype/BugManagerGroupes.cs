using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugManagerGroupes : MonoBehaviour
{
    [SerializeField] List<BugControllerGroupes> bugsController;
    [SerializeField] List<float> bugsMoveTimeController;
    [SerializeField] float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfItsGoodTime();
    }

    void CheckIfItsGoodTime ()
    {
        for(int i =0;i< bugsMoveTimeController.Count;i++)
        {
            if(time >= bugsMoveTimeController[i] && !bugsController[i].Send)
            {
                SendBugs(i);
            }
        }
    }

    void SendBugs (int i)
    {
        bugsController[i].SetTargetToBugs();
    }
}
