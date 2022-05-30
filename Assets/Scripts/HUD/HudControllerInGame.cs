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

    
    [SerializeField] TextMeshProUGUI _textTimerInGame;
    [SerializeField] TextMeshProUGUI _textTimerWin;

    [SerializeField] EventSystem eventSystem;

    [SerializeField] GameObject firstButtonDead;
    [SerializeField] GameObject firstButtunWin;

    [SerializeField] Slider slideTime;
    [SerializeField] GameObject sliderGo;

    [SerializeField] GameObject doubleJumpIcon;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] Rigidbody playerRB;
    public float deltaTime;
    private void Awake()
    {
        Instance = this;
        fpsText.text = ((int)(new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).magnitude * 3)).ToString() + " KM/H";
    }

    private void Update()
    {
        //playerSpeed.text = ((int)(new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).magnitude*3)).ToString() + " KM/H";
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + "fps";
    }
    public void OpenDeathPanel()
    {
        _deadPanel.SetActive(true);
        _inGamePanel.SetActive(false);
        _winPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        eventSystem.SetSelectedGameObject(firstButtonDead);
    }

    public void OpenOptionsPanel()
    {
        _optionsPanel.SetActive(true);
        _pausePanel.SetActive(false);
        HUD_Settings.Instance.OpenButtonPanel();
    }

    public void ChangeTimerHud(float timer)
    {
        _textTimerInGame.text = Timer.FormatTime(timer);
    }

    public void OpenPauseMenu()
    {
        _deadPanel.SetActive(false);
        _inGamePanel.SetActive(false);
        _winPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        _optionsPanel.SetActive(false);
        _deadPanel.SetActive(false);
        _inGamePanel.SetActive(true);
        _winPanel.SetActive(false);
        _pausePanel.SetActive(false);
    }

    public void RestartLevel()
    {
        LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenWinPanel(float timer)
    {
        _deadPanel.SetActive(false);
        _inGamePanel.SetActive(false);
        _winPanel.SetActive(true);
        _textTimerWin.text = Timer.FormatTime(timer);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        eventSystem.SetSelectedGameObject(firstButtunWin);
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

    public void DoubleJumpShow(bool a )
    {
        doubleJumpIcon.SetActive(a);
    }
}
