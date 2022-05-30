using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupesBugs
{
    [SerializeField] List<BugTargetController> groupeBugs;
    [SerializeField] List<Transform> groupeBugsSpawnPoint;
    
    [SerializeField] float timeToGo;
    [SerializeField] List<Transform> target;

    public void InitGroupesBugs()
    {
        for (int i = 0; i < groupeBugs.Count; i++)
        {
            groupeBugs[i].IndexGroupesBug = i;
            groupeBugs[i].GroupesBugsAssign = this;
        }
    }

    public float TimeToGo
    {
        get
        {
            return timeToGo;
        }
    }
    public void SetTarget (int index)
    {
        if (index > -1 && index < target.Count)
        {
            for (int i = 0; i < groupeBugs.Count; i++)
            {
                groupeBugs[i].Target = target[index];
                groupeBugs[i].IndexTarget = index;
            }
        }
    }

    public void ChangeTarget (int actualIndex, int indexGroupes)
    {
        if(indexGroupes >-1 && indexGroupes < groupeBugs.Count)
        {
            if(actualIndex <0)
            {
                actualIndex = 0;
            }

            if(actualIndex + 1 < target.Count)
            {
                groupeBugs[indexGroupes].IndexTarget = actualIndex + 1;
                groupeBugs[indexGroupes].Target = target[actualIndex + 1];
            }
            else
            {
                groupeBugs[indexGroupes].Target = null;
            }
        }

    }

    public void InstanceBug()
    {
        for(int i =0;i< groupeBugsSpawnPoint.Count;i++)
        {
            if (groupeBugs.Count > i && groupeBugs[i] != null)
            {
                groupeBugs[i].transform.position = groupeBugsSpawnPoint[i].position;
            }
            else
            {
                Debug.Log("peut pas faire spawn");
            }
        }
    }



}
