using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;
using Doozy.Runtime.UIManager.Components;
using UnityEngine.EventSystems;

public class HUD_Settings : MonoBehaviour
{
    public static HUD_Settings Instance;
    [SerializeField] EventSystem eventSystem;

    private void Awake()
    {
        Instance = this;
    }

    void ChangeStateMenu()
    {
        if (HudControllerInGame.Instance != null)
            HudControllerInGame.Instance.StateMenu = ActualMenu.InSettings;

        if (HUD_MainMenu.Instance != null)
            HUD_MainMenu.Instance.State = StateMainMenu.InPanelSettings;
    }

    void CloseAllPanel()
    {
        _panelControl.SetActive(false);
        _panelAudio.SetActive(false);
        _panelButton.SetActive(false);
        _panelGamePad.SetActive(false);
        _panelGraphics.SetActive(false);
    }

    #region GRAPHICS PANEL
    [Header("GRAPHICS SETTINGS")]
    [SerializeField] GameObject _panelGraphics;
    [SerializeField] TMP_Dropdown _resolutionDropDown;
    [SerializeField] TMP_Dropdown _dropDownScreenMode;
    [SerializeField] TMP_Dropdown _antiAliasingDropDown;
    [SerializeField] TMP_Dropdown _dropDownFpsTarget;
    [SerializeField] TMP_Dropdown _dropDownQuality;
    [SerializeField] UIToggle _toggleVSync;
    [SerializeField] UIToggle _toggleShowFps;
    [SerializeField] UIToggle _toggleCameraShake;
    [SerializeField] UIToggle _toggleCameraClamp;
    [SerializeField] GameObject _firstBoutonPanelGraphics;

    private int countRes;


    public void OpenGraphicsPanel()
    {
        ChangeStateMenu();
        CloseAllPanel();
        _panelGraphics.SetActive(true);
        InitializeGraphicsOptions();
        eventSystem.SetSelectedGameObject(_firstBoutonPanelGraphics);
    }

    void InitializeGraphicsOptions()
    {
        _toggleCameraShake.isOn = Settings.UseCameraShake;
        _toggleCameraClamp.isOn = Settings.UseCameraClamp;
        _toggleShowFps.isOn = Settings.ShowFps;
        _toggleVSync.isOn = Settings.UseVsync;
        _dropDownFpsTarget.value = Settings.FpsTarget;
        _dropDownFpsTarget.RefreshShownValue();
        _dropDownQuality.value = Settings.Quality;
        _antiAliasingDropDown.value = Settings.AntiAliasing;
        _dropDownScreenMode.value = Settings.ScreenMode;
        _dropDownScreenMode.RefreshShownValue();
        AddResolution(Settings.StoreResolution);
        ResolutionInitialize(Settings.StoreResolution);
    }

    public void SetCameraShake()
    {
        Settings.ChangeUseCameraShake(_toggleCameraShake.isOn);
    }

    public void SetCameraClamp()
    {
        Settings.ChangeUseCameraClamp(_toggleCameraClamp.isOn);
    }

    public void SetShowFps()
    {
        Settings.ChangeShowFps(_toggleShowFps.isOn);
    }

    public void SetVSync()
    {
        Settings.ChangeVSync(_toggleVSync.isOn);
    }

    public void SetFpsTarget()
    {
        Settings.ChangeFpsTarget(_dropDownFpsTarget.value);
    }

    public void SetQuality()
    {
        Settings.ChangeQuality(_dropDownQuality.value);
    }

    public void SetAntiAliasing()
    {
        Settings.ChangeAntiAliasing(_antiAliasingDropDown.value);
    }

    public void SetScreenMode()
    {
        Settings.ChangeScreenMode(_dropDownScreenMode.value);
    }

    public void SetResolution()
    {
        Settings.ChangeResolution(_resolutionDropDown.value);
    }

    void AddResolution(Resolution[] res)
    {
        countRes = 0;
        for (int i = 0; i < res.Length; i++)
        {
            if (res[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                Settings.StoreResolution[countRes] = res[i];
                countRes++;
            }
        }

        for (int i = 0; i < countRes; i++)
        {
            _resolutionDropDown.options.Add(new TMP_Dropdown.OptionData(ResolutionToStrings(Settings.StoreResolution[i])));
        }
    }

    void ResolutionInitialize(Resolution[] res)
    {
        for (int i = 0; i < res.Length; i++)
        {
            if (Screen.width == res[i].width && Screen.height == res[i].height)
            {
                _resolutionDropDown.value = i;
            }
        }
        _resolutionDropDown.RefreshShownValue();
    }

    private string ResolutionToStrings(Resolution resolution)
    {
        return resolution.width + " x " + resolution.height;
    }
    #endregion

    #region MENU OPTIONS
    [Header("MENU SETTINGS")]
    [SerializeField] GameObject _panelButton;
    [SerializeField] GameObject _firstButtonOptionMenu;
    public void OpenOptionsPanel()
    {
        CloseAllPanel();
        _panelButton.SetActive(true);
        eventSystem.SetSelectedGameObject(_firstButtonOptionMenu);
    }
    #endregion

    #region AUDIO PANEL

    [Header("AUDIO SETTINGS")]
    [SerializeField] GameObject _panelAudio;
    [SerializeField] Slider _sliderVolumeSoundEffect;
    [SerializeField] Slider _sliderVolumeMusic;
    [SerializeField] Slider _sliderVolumeGeneral;
    [SerializeField] GameObject _firstBoutonPanelAudio;
    public void OpenPanelAudio()
    {
        CloseAllPanel();
        _panelAudio.SetActive(true);
        ChangeStateMenu();
        InitializeAudioPanel();
        eventSystem.SetSelectedGameObject(_firstBoutonPanelAudio);
    }

    //Change Slider Value From Settings
    void InitializeAudioPanel()
    {
        _sliderVolumeGeneral.value = Settings.VolumeGeneral;
        _sliderVolumeMusic.value = Settings.VolumeMusic;
        _sliderVolumeSoundEffect.value = Settings.VolumeSFX;
    }
    //Slider Audio General
    public void ChangeAudioGeneral(float value)
    {
        Settings.ChangeVolumeGeneral(value);
    }
    //Slider Volume Music
    public void ChangeAudioMusic(float value)
    {
        Settings.ChangeVolumeMusic(value);
    }

    public void ChangeAudioSFX(float value)
    {
        Settings.ChangeVolumeSFX(value);
    }
    #endregion

    #region KEYBOARD SETTINGS PANEL
    [Header("KEYBOARD SETTINGS")]
    [SerializeField] GameObject _panelControl;
    [SerializeField] Slider _sliderSensibilityMouse;
    [SerializeField] GameObject _firstBoutonPanelKeyboard;

    private void OpenControlPanel()
    {
        CloseAllPanel();
        _panelControl.SetActive(true);
        ChangeStateMenu();
        InitializeKeybordPanel();
        eventSystem.SetSelectedGameObject(_firstBoutonPanelKeyboard);
    }

    private void InitializeKeybordPanel()
    {
        _sliderSensibilityMouse.value = Settings.SensibilityMouse;  
    }

    public void ChangeValueSensibilityMouse(float value)
    {
        Settings.ChangeValueSensibilityMouse(value);
    }

    #endregion

    #region GamePad Settings
    [Header("GAMEPAD SETTINGS")]
    [SerializeField] GameObject _panelGamePad;
    [SerializeField] UIToggle _toggleRumbler;
    [SerializeField] Slider _sliderSensibilityGamePad;
    [SerializeField] GameObject _firstButtonGamePad;
    public void OpenGamePadPanel()
    {
        CloseAllPanel();
        _panelGamePad.SetActive(true);
        ChangeStateMenu();
        InitializeGamePadPanel();
        eventSystem.SetSelectedGameObject(_firstButtonGamePad);
    }

    public void SetRumbler()
    {
        Settings.ChangeUseRumbler(_toggleRumbler.isOn);
    }

    public void ChangeValueSensibilityGamePad(float value)
    {
        Settings.ChangeValueSensibilityGamePad(value);
    }

    void InitializeGamePadPanel()
    {
        _toggleRumbler.isOn = Settings.UseRumbler;
        _sliderSensibilityGamePad.value = Settings.SensibilityGamePad;
    }
    #endregion
}
