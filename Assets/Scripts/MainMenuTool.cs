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
        Time.timeScale = 1;
        //InitializeTeleport();

        DATA data = Data_Manager.Instance.GetData();

        for (int i = 0; i < data._worldData.Count; i++)
        {
            if (data._worldData[i].HaveUnlockWorld)
            {
                Debug.Log("WorldUnlock");
                if (data._worldData[i]._mapData[data._worldData[i]._mapData.Count - 1].GetHaveUnlockLevel())
                {
                    Debug.Log("MapUnlco");
                    if (i + 1 < data._worldData.Count)
                    {
                        Debug.Log("next level ok");
                        if (data._worldData[i + 1].HaveUnlockWorld == false)
                        {
                            
                            LaunchCinematiqueLevel(i + 1);
                            return;
                        }
                    }
                }
            }
        }
    }

    private void LaunchCinematiqueLevel(int world)
    {
        Debug.Log("not unlock next world");
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

