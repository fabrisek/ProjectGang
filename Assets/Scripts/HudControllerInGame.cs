using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudControllerInGame : MonoBehaviour
{
    public static HudControllerInGame Instance;
    [SerializeField] GameObject _deadPanel;
    [SerializeField] GameObject _winPanel;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenDeathPanel()
    {
        _deadPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenWinPanel()
    {
        _winPanel.SetActive(true);
    }
}
