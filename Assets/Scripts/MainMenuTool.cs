using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTool : MonoBehaviour
{
    [SerializeField] GameObject playerGo;
    [SerializeField] Transform[] allTeleportPoint;

    // Start is called before the first frame update
    void Start()
    {
        //InitializeTeleport();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* public void InitializeTeleport()
    {
        DATA data = Data_Manager.Instance.GetData();

        for (int i = 0; i < data._mapData.Count; i++)
        {
            if(data._mapData[i].GetHaveUnlockLevel() == false)
            {
                Teleport(i);
                print(i);
                return;
            }
        }

        Teleport(data._mapData.Count - 1);
    }*/

    public void Teleport(int indexTp)
    {
        playerGo.transform.position = allTeleportPoint[indexTp].position;
    }
}

