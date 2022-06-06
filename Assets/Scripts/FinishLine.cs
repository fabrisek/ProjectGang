using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public static FinishLine Instance;

    public bool isWin;
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] int _levelIndex;
    [SerializeField] int _worldIndex;

    public int LevelIndex
    {
        get
        {
            return _levelIndex;
        }
    }
    public int WorldIndex
    {
        get
        {
            return _worldIndex;
        }
    }
    public void FinishLevel()
    {
        if (PlayerDeath.Instance.GetIsDead() == false)
        {
            Time.timeScale = 0;
            Timer.Instance.StopTimer();
            if (!isWin)
            {
                isWin = true;
                float timer = Timer.Instance.GetTimer();

                if (Data_Manager.Instance)
                {                    
                    Data_Manager.Instance.SetRecord(timer, _levelIndex, _worldIndex);
                    HudControllerInGame.Instance.OpenWinPanel(timer, Data_Manager.Instance.GetMapData(_levelIndex, _worldIndex).GetHighScore(), _levelIndex, _worldIndex);                   
                }

                else if (Data_Manager.Instance == null)
                {
                    HudControllerInGame.Instance.OpenWinPanel(timer, timer, _levelIndex, _worldIndex);
                }
            }
        }    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            AudioManager.instance.playSoundEffect(9, 0.3f);
            FinishLevel();

            if (other.gameObject.GetComponentInChildren<PlayerCam>())
                other.gameObject.GetComponentInChildren<PlayerCam>().enabled = false;
            }
    }
}
