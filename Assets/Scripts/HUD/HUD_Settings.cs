using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Components;
using UnityEngine.EventSystems;

public class HUD_Settings : MonoBehaviour
{
    [SerializeField] UIContainer audiopanel;
    [SerializeField] UIContainer keyboardpanel;
    [SerializeField] UIContainer gamepadPanel;
    [SerializeField] UIContainer graphicsPanel;
    [SerializeField] UIContainer buttonPanel;
    public void CloseSettings()
    {
        audiopanel.Hide();
        keyboardpanel.Hide();
        gamepadPanel.Hide();
        graphicsPanel.Hide();
        buttonPanel.Show();

        if (HudControllerInGame.Instance != null)
            HudControllerInGame.Instance.StateMenu = ActualMenu.SettingsMenu;
    }
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


    #region GRAPHICS PANEL
    [Header("GRAPHICS SETTINGS")]
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
        InitializeGraphicsOptions();
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
        ChangeStateMenu();
        InitializeAudioPanel();
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

    private void OpenControlPanel()
    {
        ChangeStateMenu();
        InitializeKeybordPanel();
    }

    private void InitializeKeybordPanel()
    {
        _sliderSensibilityMouse.value = Settings.SensibilityMouse;  
    }

    public void ChangeValueSensibilityMouse()
    {
        Settings.ChangeValueSensibilityMouse(_sliderSensibilityMouse.value);
    }

    #endregion

    #region GamePad Settings
    [Header("GAMEPAD SETTINGS")]
    [SerializeField] UIToggle _toggleRumbler;
    [SerializeField] Slider _sliderSensibilityGamePad;
    public void OpenGamePadPanel()
    {
        ChangeStateMenu();
        InitializeGamePadPanel();
    }

    public void SetRumbler()
    {
        Settings.ChangeUseRumbler(_toggleRumbler.isOn);
    }

    public void ChangeValueSensibilityGamePad()
    {
        Settings.ChangeValueSensibilityGamePad(_sliderSensibilityGamePad.value);
    }

    void InitializeGamePadPanel()
    {
        _toggleRumbler.isOn = Settings.UseRumbler;
        _sliderSensibilityGamePad.value = Settings.SensibilityGamePad;
    }
    #endregion
}
