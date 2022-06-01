using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupesBugs
{
    [SerializeField] List<BugTargetController> groupeBugs;
   
    
    [SerializeField] List<Transform> target;
    [SerializeField] Transform refPositionDistPlayer;

    public void InitGroupesBugs()
    {
        for (int i = 0; i < groupeBugs.Count; i++)
        {
            groupeBugs[i].enabled = true;
          //  Debug.Log(groupeBugs[i].isActiveAndEnabled);
            groupeBugs[i].IndexGroupesBug = i;
            groupeBugs[i].GroupesBugsAssign = this;
            
        }

        SetTarget(0);
        
    }

    public Transform RefPositionDistPlayer
    {
        get
        {
            return refPositionDistPlayer;
        }
    }

    public void SetTarget (int index)
    {
        if (index > -1 && index < target.Count)
        {
            for (int i = 0; i < groupeBugs.Count; i++)
            {
                groupeBugs[i].enabled = true;
                groupeBugs[i].Target = target[index];
                groupeBugs[i].IndexTarget = index;
             
            }
        }

    }

    public void ResetAllTargetSet ()
    {
        for (int i = 0; i < groupeBugs.Count; i++)
        {
            
            groupeBugs[i].Target = null;
            groupeBugs[i].IndexTarget = 0;

            groupeBugs[i].enabled = false;
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
                groupeBugs[indexGroupes].enabled = false;

            }
        }

    }

    public void DestroyAllBug ()
    {
        for(int i= 0; i< groupeBugs.Count;i++)
        {
            groupeBugs[i].DestroyMe();
        }
        groupeBugs = new List<BugTargetController>();
    }



}
