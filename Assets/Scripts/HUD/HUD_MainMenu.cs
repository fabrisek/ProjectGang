using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
public enum StateMainMenu
{
    Menu,
    Settings,
    InPanelSettings,
    InGame,
    InPanelGame
}
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

    [SerializeField] GameObject panelSelector;
    [SerializeField] Transform parentSelector;
    [SerializeField] GameObject CardWorldPrefab;
    [SerializeField] TextMeshProUGUI worldName;
    [SerializeField] TextMeshProUGUI starText;

    public StateMainMenu State { get; set; }

    private void Awake()
    {
        Instance = this;
    }
    public void OpenLevelSelector()
    {
        panelSelector.SetActive(false);
        _mainMenuPanel.SetActive(false);
        _levelSelectionPanel.SetActive(true);
        antController.enabled = true;
        State = StateMainMenu.InGame;
    }

    public void OpenPanelSelectionLevel(int worldIndex)
    {
        State = StateMainMenu.InPanelGame;
        panelSelector.SetActive(true);
        worldName.text = Data_Manager.Instance.GetData()._worldData[worldIndex].WorldName;

        int totalStar = 0;
        int starUnlock = 0;

        foreach (var item in panelSelector.GetComponentsInChildren<CardWorld>())
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < Data_Manager.Instance.GetData()._worldData[worldIndex]._mapData.Count; i++)
        {
            GameObject cardObj = Instantiate(CardWorldPrefab, parentSelector);
            int starLevel = 0;
            MapData mapData = Data_Manager.Instance.GetData()._worldData[worldIndex]._mapData[i];
            for (int j = 0; j < mapData.TimeStar.Length; j++)
            {
                totalStar++;
                if (mapData.GetHighScore() <= mapData.TimeStar[j] && mapData.GetHighScore() != 0)
                {
                    starLevel++;
                    starUnlock++;
                }
            }

            cardObj.GetComponent<CardWorld>().ChangeInformation(mapData.spriteLevel, mapData.GetHighScore(), mapData.GetMapName(), (i + 1).ToString(), starLevel, mapData.GetIndexScene(), mapData.GetHaveUnlockLevel());
        }
        starText.text = "STAR : " + starUnlock.ToString() + " / " + totalStar.ToString();
    }

    public void OpenSettings()
    {
        panelSelector.SetActive(false);
        State = StateMainMenu.Settings;
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(firstButtonSettings);
        HUD_Settings.Instance.OpenOptionsPanel();
    }

    public void Back()
    {



            switch (State)
            {
                case StateMainMenu.InPanelGame:
                    OpenLevelSelector();
                    break;
                case StateMainMenu.InGame:
                    CloseSettings();
                    break;
                case StateMainMenu.Settings:
                    CloseSettings();
                    break;
                case StateMainMenu.InPanelSettings:
                    OpenSettings();
                    break;
            
        }
    }

    public void CloseSettings()
    {
        panelSelector.SetActive(false);
        State = StateMainMenu.Menu;
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
        if (Data_Manager.AlreadyInGame == false)
            CloseSettings();
        else
            OpenLevelSelector();
    }
}
