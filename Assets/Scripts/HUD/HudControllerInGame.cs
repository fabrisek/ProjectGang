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

    
    [SerializeField] TextMeshProUGUI _textTimerInGame;
    [SerializeField] TextMeshProUGUI _textTimerWin;

    [SerializeField] EventSystem eventSystem;

    [SerializeField] GameObject firstButtonDead;
    [SerializeField] GameObject firstButtunWin;

    [SerializeField] Slider slideTime;
    [SerializeField] GameObject sliderGo;

    [SerializeField] GameObject doubleJumpIcon;
    [SerializeField] TextMeshProUGUI playerSpeed;
    [SerializeField] Rigidbody playerRB;
    
    private void Awake()
    {
        Instance = this;
        playerSpeed.text = ((int)(new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).magnitude * 3)).ToString() + " KM/H";
    }

    private void Update()
    {
        playerSpeed.text = ((int)(new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).magnitude*3)).ToString() + " KM/H";
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
