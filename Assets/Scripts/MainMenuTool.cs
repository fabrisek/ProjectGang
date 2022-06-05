using UnityEngine;

public class MainMenuTool : MonoBehaviour
{
    [SerializeField] GameObject playerGo;
    [SerializeField] Transform[] allTeleportPoint;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        InitializeTeleport();

        DATA data = Data_Manager.Instance.GetData();

        for (int i = 0; i < data._worldData.Count; i++)
        {
            if (data._worldData[i].HaveUnlockWorld)
            {
                if (data._worldData[i]._mapData[data._worldData[i]._mapData.Count - 1].GetHaveUnlockLevel())
                {
                    if (i + 1 < data._worldData.Count)
                    {
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
        DATA data = Data_Manager.Instance.GetData();
        data._worldData[world].HaveUnlockWorld = true;
        Debug.Log("not unlock next world");
    }

   public void InitializeTeleport()
    {
        Teleport(Data_Manager.Instance.GetData().LastWorld);
    }

    public void Teleport(int indexTp)
    {
        playerGo.transform.position = allTeleportPoint[indexTp].position;
    }
}

