using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FInishLine : MonoBehaviour
{
    [SerializeField] int _levelIndex;
    public void FinishLevel()
    {
        Timer.Instance.StopTimer();
        float timer = Timer.Instance.GetTimer();
        HudControllerInGame.Instance.OpenWinPanel(timer);
        PlayFabHighScore.instance.SendLeaderBord(timer, SceneManager.GetActiveScene().ToString());
        Data_Manager.Instance.SetRecord(timer, _levelIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            FinishLevel();
        }
    }
}
