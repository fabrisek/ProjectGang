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
    public void FinishLevel()
    {
        if (PlayerDeath.Instance.GetIsDead() == false)
        {
            isWin = true;
            Timer.Instance.StopTimer();
            float timer = Timer.Instance.GetTimer();
            HudControllerInGame.Instance.OpenWinPanel(timer);
            if (PlayFabHighScore.Instance)
                PlayFabHighScore.Instance.SendLeaderBord(timer, SceneManager.GetActiveScene().ToString());
            //Data_Manager.Instance.SetRecord(timer, _levelIndex);
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            AudioManager.instance.playSoundEffect(9, 0.5f);
            FinishLevel();
            //other.gameObject.GetComponentInChildren<PlayerCam>().enabled = false;
            }
    }
}
