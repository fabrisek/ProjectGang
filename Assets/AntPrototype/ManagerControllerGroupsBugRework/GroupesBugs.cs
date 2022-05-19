using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupesBugs
{
    [SerializeField] List<BugTargetController> groupeBugs;
    [SerializeField] float timeToGo;
   
    public void SetTarget (Transform target)
    {
        for(int i =0; i<groupeBugs.Count;i++)
        {
            groupeBugs[i].Target = target;
        }
    }

}
