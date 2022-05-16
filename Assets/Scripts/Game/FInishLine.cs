using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] int _levelIndex;
    public void FinishLevel()
    {
        Timer.Instance.StopTimer();
        float timer = Timer.Instance.GetTimer();
        HudControllerInGame.Instance.OpenWinPanel(timer);
        if (PlayFabHighScore.instance)
            PlayFabHighScore.instance.SendLeaderBord(timer, SceneManager.GetActiveScene().ToString());
        Data_Manager.Instance.SetRecord(timer, _levelIndex);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            FinishLevel();
            other.gameObject.GetComponentInChildren<PlayerCam>().enabled = false;
        }
    }
}
