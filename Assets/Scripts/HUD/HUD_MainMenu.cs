using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

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

    public void OpenPanelSelectionLevel(int worldIndex)
    {
        panelSelector.SetActive(true);
        worldName.text = Data_Manager.Instance.GetData()._worldData[worldIndex].WorldName;

        int totalStar = 0;
        int starUnlock = 0;
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

            cardObj.GetComponent<CardWorld>().ChangeInformation(mapData.spriteLevel, mapData.GetHighScore(), mapData.GetMapName(), (i + 1).ToString(), starLevel, mapData.GetIndexScene());
        }
        starText.text = "STAR : " + starUnlock.ToString() + " / " + totalStar.ToString();
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
        HUD_Settings.Instance.OpenButtonPanel();
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
