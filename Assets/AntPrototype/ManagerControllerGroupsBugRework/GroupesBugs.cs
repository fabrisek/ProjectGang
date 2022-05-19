using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupesBugs
{
    [SerializeField] List<BugTargetController> groupeBugs;
    [SerializeField] float timeToGo;
    [SerializeField] Transform target;
   
    public float TimeToGo
    {
        get
        {
            return timeToGo;
        }
    }
    public void SetTarget ()
    {
        for(int i =0; i<groupeBugs.Count;i++)
        {
            groupeBugs[i].Target = target;
        }
    }

}
