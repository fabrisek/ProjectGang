using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FInishLine : MonoBehaviour
{
    public void FinishLevel()
    {
        Timer.Instance.StopTimer();
        float timer = Timer.Instance.GetTimer();
        HudControllerInGame.Instance.OpenWinPanel();
        PlayFabHighScore.instance.SendLeaderBord(timer, SceneManager.GetActiveScene().ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            FinishLevel();
        }
    }
}
