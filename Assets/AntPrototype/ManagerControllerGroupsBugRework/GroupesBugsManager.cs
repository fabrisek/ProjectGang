using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupesBugsManager : MonoBehaviour
{
    [SerializeField] List<GroupesBugs> groupesBugs;

    List<GroupesBugs> groupesBugsGo;

    [SerializeField] float time;

    public float Time
    {
        set
        {
            time = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        groupesBugsGo = new List<GroupesBugs>();
        for(int i =0;i<groupesBugs.Count;i++)
        {
            groupesBugs[i].InitGroupesBugs();
        }

        StartCoroutine(CoroutineSetBug());
    }

    IEnumerator CoroutineSetBug ()
    {

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < groupesBugs.Count; i++)
        {
            groupesBugs[i].ResetAllTargetSet();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(groupesBugs.Count != 0)
        {
            SetTargetToGroupeBug();
        }
        
    }

    void ChangeToOtherList (int index)
    {
        groupesBugsGo.Add(groupesBugs[index]);
        List<GroupesBugs> groupesBugsTemps = groupesBugs;
        groupesBugs = new List<GroupesBugs>();
        for (int i = 0; i< groupesBugsTemps.Count;i++)
        {
           
            if(i!=index)
            {
                groupesBugs.Add(groupesBugsTemps[i]);
               
            }
        }
    }

    int CheckIfTimeToGo ()
    {

        for(int i = 0;i<groupesBugs.Count;i++)
        {
            if(time >= groupesBugs[i].TimeToGo)
            {
                return i;
            }
        }

        return -1;
    }

    void SetTargetToGroupeBug ()
    {
        int index = CheckIfTimeToGo();
        if(index != -1)
        {
            groupesBugs[index].InstanceBug();
            groupesBugs[index].SetTarget(0);
            ChangeToOtherList(index);
        }
    }

    
}
