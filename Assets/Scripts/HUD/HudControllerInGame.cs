using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HudControllerInGame : MonoBehaviour
{
    public static HudControllerInGame Instance;
    [SerializeField] GameObject _deadPanel;
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _inGamePanel;

    [SerializeField] TextMeshProUGUI _textTimerInGame;
    [SerializeField] TextMeshProUGUI _textTimerWin;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenDeathPanel()
    {
        _deadPanel.SetActive(true);
    }

    public void ChangeTimerHud(float timer)
    {
        _textTimerInGame.text = Timer.FormatTime(timer);
    }

    public void RestartLevel()
    {
        LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenWinPanel(float timer)
    {
        _winPanel.SetActive(true);
        _textTimerWin.text = Timer.FormatTime(timer);
    }
}
