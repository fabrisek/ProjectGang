using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupesBugsManager : MonoBehaviour
{
    [SerializeField] List<GroupesBugs> groupesBugs;

    List<GroupesBugs> groupesBugsGo;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
