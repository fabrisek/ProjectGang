using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GroupesBugsManager groupesBugsManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetTimeTOBugsManager();
    }

    void SetTimeTOBugsManager ()
    {
        if(groupesBugsManager != null)
        {
            groupesBugsManager.Time = Timer.Instance.GetTimer();
        }
    }




}
