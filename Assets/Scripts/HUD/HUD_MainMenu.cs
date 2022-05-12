using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HUD_MainMenu : MonoBehaviour
{
    public static HUD_MainMenu Instance;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _levelSelectionPanel;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenLevelSelector()
    {
        _mainMenuPanel.SetActive(false);
        _levelSelectionPanel.SetActive(true);
    }

    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        _levelSelectionPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        CloseSettings();
    }
}
