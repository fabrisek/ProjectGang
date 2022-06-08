using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HudControllerInGame : MonoBehaviour
{
    public static HudControllerInGame Instance;
    [SerializeField] GameObject _deadPanel;
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _inGamePanel;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _optionsPanel;

    public void OpenInGamePanel()
    {
        _inGamePanel.SetActive(true);
    }


    [SerializeField] TextMeshProUGUI _textTimerInGame;
    [SerializeField]
    TextMeshProUGUI _textTimeDead;
    [SerializeField] TextMeshProUGUI _textTimerWin;
    [SerializeField] TextMeshProUGUI _textBestTime;
    [SerializeField]
    Image[] allStar;
    [SerializeField] Sprite starUnlock;
    [SerializeField] Sprite startLock;
    [SerializeField] GameObject buttonNextLevel;
    private int indexNextScene;
    private int indexLevel;
    private int indexWorld;
    [SerializeField] EventSystem eventSystem;

    [SerializeField] GameObject firstButtonDead;
    [SerializeField] GameObject firstButtunWin;
    [SerializeField] GameObject firstButtunPause;

    [SerializeField] Slider slideTime;
    [SerializeField] GameObject sliderGo;

    [SerializeField] GameObject doubleJumpIcon;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] Rigidbody playerRB;

    [SerializeField] TextMeshProUGUI threeTwoOneSlider;
    [SerializeField] string[] txtThreeTwoOne;
    [SerializeField] public AnimationCurve curveChangeLetter;
    [SerializeField] float maxSizeFont;

    float timeToAnimLetter;
    float timeToReacToAnimLetterh;
    bool startTimeToAimLetter;

    [SerializeField] GameObject parentHighScore;
    [SerializeField] GameObject panelHighScore;
    [SerializeField] GameObject HighScorePrefab;
    [SerializeField] TextMeshProUGUI textPosBouton;
    bool showFps;
    public void  SetShowFps(bool show) 
    {
        showFps = show;
        fpsText.enabled = show;
    }
    public ActualMenu StateMenu { get; set; }
    public bool InMenu { get; set; }
    public float deltaTime;

    private void Awake()
    {
        Instance = this;
        
    }

    private void Start()
    {
        InitLetterAnim();
        _optionsPanel.SetActive(false);
        SetShowFps(Settings.ShowFps);
    }

    private void Update()
    {
        if (Settings.ShowFps)
        {
            //playerSpeed.text = ((int)(new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).magnitude*3)).ToString() + " KM/H";
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString() + "fps";
            UpdateTimerForLetterAnim();
        }

    }
    public void OpenDeathPanel()
    {
        _deadPanel.SetActive(true);
        _inGamePanel.SetActive(false);
        _winPanel.SetActive(false);

        if (InputManager.currentControlDevice == ControlDeviceType.KeyboardAndMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        eventSystem.SetSelectedGameObject(firstButtonDead);
        _textTimeDead.text = "TIMER : " + Timer.FormatTime(Timer.Instance.GetTimer());
    }

    public void Back()
    {
        if (StateMenu == ActualMenu.Pause)
        {
            PlayerMovementAdvanced.Instance.Pause();
        }
        if (StateMenu == ActualMenu.SettingsMenu)
        {
            OpenPauseMenu();
        }
        if (StateMenu == ActualMenu.InSettings)
        {
            OpenOptionsPanel();
        }
    }

    public void OpenOptionsPanel()
    {
        StateMenu = ActualMenu.SettingsMenu;
        _optionsPanel.SetActive(true);
        _pausePanel.SetActive(false);
    }

    public void ChangeTimerHud(float timer)
    {
        _textTimerInGame.text = Timer.FormatTime(timer);
    }

    public void OpenPauseMenu()
    {
        StateMenu = ActualMenu.Pause;
        InMenu = true;
        _deadPanel.SetActive(false);
        _inGamePanel.SetActive(false);
        _winPanel.SetActive(false);
        _pausePanel.SetActive(true);
        _optionsPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(firstButtunPause);
    }

    public void ClosePauseMenu()
    {
        InMenu = false;
        _optionsPanel.SetActive(false);
        _deadPanel.SetActive(false);
        _inGamePanel.SetActive(true);
        _winPanel.SetActive(false);
        _pausePanel.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenWinPanel(float timer, float bestTime, int levelIndex,int worldIndex)
    {
        _deadPanel.SetActive(false);
        _inGamePanel.SetActive(false);
        _winPanel.SetActive(true);
        _textTimerWin.text = "TIME : " + Timer.FormatTime(timer);

        if (InputManager.currentControlDevice == ControlDeviceType.KeyboardAndMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        eventSystem.SetSelectedGameObject(firstButtunWin);

        _textBestTime.text = "BEST TIME : " + Timer.FormatTime(bestTime);

        if (bestTime == 0)
        {
            _textBestTime.text = "BEST TIME : " + Timer.FormatTime(timer);
        }
        if (timer == bestTime)
        {
            _textBestTime.text = "NEW RECORD : " + Timer.FormatTime(bestTime);
            _textBestTime.color = Color.red;
        }

        if (Data_Manager.Instance != null)
        {
            DATA data = Data_Manager.Instance.GetData();

                if (data._worldData[worldIndex]._mapData.Count - 1 == levelIndex)
                {
                    buttonNextLevel.SetActive(false);
                }

            else
            {
                indexWorld = worldIndex;
                buttonNextLevel.SetActive(true);
                indexNextScene = levelIndex + 1;
                indexLevel = levelIndex;
            }

            for (int i = 0; i < data._worldData[worldIndex]._mapData[levelIndex].GetSceneData().TimeStar.Length; i++)
            {
                if (data._worldData[worldIndex]._mapData[levelIndex].GetHighScore() <= data._worldData[worldIndex]._mapData[levelIndex].GetSceneData().TimeStar[i])
                {
                    allStar[i].sprite = starUnlock;
                }
                else
                {
                    allStar[i].sprite = startLock;
                }
            }
        }
        
        if (Data_Manager.Instance == null)
        {
            buttonNextLevel.SetActive(false);
        }

        if (PlayFabHighScore.Instance != null)
        {
            if (PlayFabLogin.Instance != null && PlayFabLogin.Instance.GetEntityId() != "")
                StartCoroutine(WaitPosPlayer());
        }

    }

    IEnumerator WaitPosPlayer()
    {
        yield return new WaitForSecondsRealtime(.5f);
        PlayFabHighScore.Instance.GetPosPlayer(Data_Manager.Instance.GetData()._worldData[indexWorld]._mapData[indexLevel].GetSceneData().MapName);
    }

    public void ChangePosPlayer(int pos)
    {
        textPosBouton.text = "TOP # "+ (pos + 1).ToString();
    }

    public void CloseHighScore()
    {
        panelHighScore.SetActive(false);
    }

    public void ClickButtonHighScore()
    {
        panelHighScore.SetActive(true);
        PlayFabHighScore.Instance.InitializeHighScore(HighScorePrefab, parentHighScore.transform);
        PlayFabHighScore.Instance.GetLeaderBord(Data_Manager.Instance.GetData()._worldData[indexWorld]._mapData[indexLevel].GetSceneData().MapName);
    }

    public void ClickButtonAroundPlayer()
    {
        panelHighScore.SetActive(true);
        if (PlayFabHighScore.Instance != null)
        {

        PlayFabHighScore.Instance.InitializeHighScore(HighScorePrefab, parentHighScore.transform);
        PlayFabHighScore.Instance.GetLeaderBoardAroundPlayer(Data_Manager.Instance.GetData()._worldData[indexWorld]._mapData[indexLevel].GetSceneData().MapName);
        }
        else
        {
            Debug.LogWarning("Playfab Highscore not Initialized");
        }
    }

    public void OpenNextLevel()
    {
        LevelLoader.Instance.LoadLevel(Data_Manager.Instance.GetMapData(indexNextScene, indexWorld).GetSceneData().IndexScene);
    }

    public void OpenMainMenu()
    {
        LevelLoader.Instance.LoadLevel(0);
        AudioManager.instance.PlayMusic(0);
    }

    public void ChangeSliderTimeValue(float value , float maxValue, bool doActive)
    {
        sliderGo.SetActive(doActive);
        slideTime.value = value;
        slideTime.maxValue = maxValue;

    }

    public void DoubleJumpShow(bool a)
    {
        doubleJumpIcon.SetActive(a);
    }


    void InitLetterAnim()
    {
        timeToAnimLetter = 0;
        startTimeToAimLetter = false;
        threeTwoOneSlider.enabled = false;
    }

    void UpdateTimerForLetterAnim()
    {
        if (startTimeToAimLetter)
        {
            timeToAnimLetter = TimerLetterChange(timeToReacToAnimLetterh, timeToAnimLetter);
            ChangeLettersSizeAndAlpha();
        }
    }

    public void StartThreeTwoOne(float time)
    {
        startTimeToAimLetter = true;
        timeToReacToAnimLetterh = time / 3;
        threeTwoOneSlider.enabled = true;
        threeTwoOneSlider.text = txtThreeTwoOne[0];
        AudioManager.instance.playSoundEffect(17, 1);
        StopCoroutine(CoroutineAffichageImagesStart(time, 1));
        StartCoroutine(CoroutineAffichageImagesStart(time, 1));

    }

    float TimerLetterChange(float timeToReach, float time)
    {
        if (time < timeToReach)
        {
            time += Time.deltaTime;
        }
        else
        {

            startTimeToAimLetter = false;
        }
        return time;
    }

    void ChangeLettersSizeAndAlpha()
    {
        threeTwoOneSlider.color = new Color(threeTwoOneSlider.color.r, threeTwoOneSlider.color.g, threeTwoOneSlider.color.b, curveChangeLetter.Evaluate(timeToAnimLetter / timeToReacToAnimLetterh));
        threeTwoOneSlider.fontSize = maxSizeFont * curveChangeLetter.Evaluate(timeToAnimLetter / timeToReacToAnimLetterh);
    }



    IEnumerator CoroutineAffichageImagesStart(float time, int index)
    {
        threeTwoOneSlider.enabled = true;
        yield return new WaitForSeconds(time / 3);

        //Reset les lettre au debu de la curve
        this.timeToAnimLetter = 0;
        ChangeLettersSizeAndAlpha();

        //Affichage de la nouvelle lettre
        threeTwoOneSlider.text = txtThreeTwoOne[index];
        if(index == 3)
        {
            AudioManager.instance.playSoundEffect(18, 1);
        }
        else
        {
            AudioManager.instance.playSoundEffect(17, 1);
        }
        

        //set timer pour anim la lettre
        startTimeToAimLetter = true;
        timeToReacToAnimLetterh = time / 3;

        if (index + 1 < txtThreeTwoOne.Length)
        {

            StartCoroutine(CoroutineAffichageImagesStart(time, index + 1));
        }
        else
        {
            StartCoroutine(CoroutineRemoveTextGo(time));
        }
    }

    IEnumerator CoroutineRemoveTextGo(float time)
    {
        yield return new WaitForSeconds(time / 3);

        threeTwoOneSlider.enabled = false;
        threeTwoOneSlider.text = null;
    }
}
public enum ActualMenu
{
    Pause,
    SettingsMenu,
    InSettings
}

