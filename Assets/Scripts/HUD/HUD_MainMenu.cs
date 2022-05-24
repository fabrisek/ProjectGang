using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class HUD_MainMenu : MonoBehaviour
{
    public static HUD_MainMenu Instance;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _levelSelectionPanel;
    [SerializeField] EventSystem eventSystem;

    [SerializeField] MenuAntCrontroller antController;
    [SerializeField] GameObject firstButtonMenu;
    [SerializeField] GameObject firstButtonSettings;
    [SerializeField] GameObject firstButtonInGame;
 
    private void Awake()
    {
        Instance = this;
    }
    public void OpenLevelSelector()
    {
        _mainMenuPanel.SetActive(false);
        _levelSelectionPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(firstButtonInGame);
        antController.enabled = true;
    }

    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(firstButtonSettings);
    }

    public void CloseSettings()
    {
        eventSystem.SetSelectedGameObject(firstButtonMenu);
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        _levelSelectionPanel.SetActive(false);
        antController.enabled = false;
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
