using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupesBugsManager : MonoBehaviour
{
    [SerializeField] List<GroupesBugs> groupesBugs;

    List<GroupesBugs> groupesBugsGo;

    [SerializeField] float distToGo;

    Transform playerpPos;

    public Transform Player
    {
        set
        {
            playerpPos = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        groupesBugsGo = new List<GroupesBugs>();
        do
        {
            for (int i = 0; i < groupesBugs.Count; i++)
            {
                groupesBugs[i].InitGroupesBugs(playerpPos);
            }
        } while (playerpPos == null);

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

        if (groupesBugsGo.Count != 0)
        {
            DestroyToGroupeBug();
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

    void SetTargetToGroupeBug ()
    {
        if (playerpPos != null)
        {
            int index = checkTheDistancePlayerRef();
            if (index != -1)
            {
                groupesBugs[index].SetTarget(0);
                ChangeToOtherList(index);
            }
        }
    }

    int checkTheDistancePlayerRef ()
    {
        for(int i =0; i< groupesBugs.Count;i++)
        {
            if(distToGo >= Vector3.Distance(new Vector3(groupesBugs[i].RefPositionDistPlayer.position.x,0, groupesBugs[i].RefPositionDistPlayer.position.z), new Vector3(playerpPos.position.x,0, playerpPos.position.z)))
            {
                return i;
            }

        }

        return -1;
    }

    void DestroyToGroupeBug()
    {
        if (playerpPos != null)
        {
            int index = checkTheDistancePlayerRefDisable();
            if (index != -1)
            {
                groupesBugsGo[index].DestroyAllBug();
            }
        }
    }

    int checkTheDistancePlayerRefDisable()
    {
        for (int i = 0; i < groupesBugsGo.Count; i++)
        {
            if (distToGo*2 <= Vector3.Distance(new Vector3(groupesBugsGo[i].RefPositionDistPlayer.position.x, 0, groupesBugsGo[i].RefPositionDistPlayer.position.z), new Vector3(playerpPos.position.x, 0, playerpPos.position.z)))
            {
                return i;
            }

        }

        return -1;
    }


}
