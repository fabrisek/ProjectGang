using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Components;
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
    [SerializeField] Camera firstCam;
    [SerializeField] Camera menuCam;
    [SerializeField] UIContainer _mainMenu;
    [SerializeField] UIContainer _settings;
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
    public void OpenMainMenu()
    {
        firstCam.enabled = false;
        menuCam.enabled = true;
        _mainMenu.Show();
        State = StateMainMenu.Menu;
    }

    public void CliclPlay()
    {
        firstCam.enabled = true;
        menuCam.enabled = false;
        antController.enabled = true;
        State = StateMainMenu.InGame;
    }

    public void OpenPanelSelectionLevel(int worldIndex)
    {
        _levelSelectionPanel.SetActive(false);
        antController.enabled = false;
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
            for (int j = 0; j < mapData.GetSceneData().TimeStar.Length; j++)
            {
                totalStar++;
                if (mapData.GetHighScore() <= mapData.GetSceneData().TimeStar[j] && mapData.GetHighScore() != 0)
                {
                    starLevel++;
                    starUnlock++;
                }
            }
            
            if (i == 0)
            {
                eventSystem.SetSelectedGameObject(cardObj.GetComponent<UIButton>().gameObject);
            }

            cardObj.GetComponent<CardWorld>().ChangeInformation(mapData.GetSceneData().spriteLevel, mapData.GetHighScore(), mapData.GetSceneData().MapName, (i + 1).ToString(), starLevel, mapData.GetSceneData().IndexScene, mapData.GetHaveUnlockLevel());
        }

        starText.text = "STAR : " + starUnlock.ToString() + " / " + totalStar.ToString();
    }

    public void OpenSettings()
    {
        _settings.Show();
        State = StateMainMenu.Settings;
    }

    public void Back()
    {
            switch (State)
            {
                case StateMainMenu.InPanelGame:
                    OpenMainMenu();
                    break;
                case StateMainMenu.InGame:
                    OpenMainMenu();
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
        _settings.Hide();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        if (Data_Manager.AlreadyInGame == false)
            OpenMainMenu();
        else
        {
            StartCoroutine(Hide());
            CliclPlay();
        }
    }

    IEnumerator Hide()
    {
            yield return new WaitForSeconds(.8f);
            _mainMenu.Hide();
    }
}
