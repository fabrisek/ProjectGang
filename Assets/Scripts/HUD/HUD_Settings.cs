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
    [SerializeField] GameObject _panelAudio;
    [SerializeField] GameObject _panelControl;
    [SerializeField] GameObject _panelGraphics;
    [SerializeField] GameObject _panelButton;

    [SerializeField] TMP_Dropdown _resolutionDropDown;
    [SerializeField] TMP_Dropdown _displayTypeDropDown;
    [SerializeField] TMP_Dropdown _antiAliasingDropDown;

    [SerializeField] UIToggle _vSyncToggle;
    [SerializeField] UIToggle _toggleShowFps;
    [SerializeField] UIToggle _toggleCameraShake;
    [SerializeField] UIToggle _toggleRumbler;
    [SerializeField] UIToggle _toggleCameraClamp;
    [SerializeField] Slider sliderSensibilityMouseX;
    [SerializeField] Slider sliderSensibilityMouseY;
    [SerializeField] TMP_Dropdown _dropDownFpsTarget;

    [SerializeField] Slider sliderVolumeSoundEffect;
    [SerializeField] Slider sliderVolumeMusic;

    [SerializeField] GameObject _firstButtonOptionMenu;
    [SerializeField]
    GameObject _firstBoutonPanelAudio;
    [SerializeField]
    GameObject _firstBoutonPanelControl;
    [SerializeField]
    GameObject _firstBoutonPanelGraphics;
    FullScreenMode screenMode;
    private Resolution[] storeResolution;
    int countRes;

    public bool UseCameraShake { get; set; }
    public bool UseRumbler { get; set; }
    public bool UseClampCamera { get; set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InitializeGraphicsOptions();
        InitializeAudioOption();
        InitializeSensibility();
        
        sliderVolumeSoundEffect.value = PlayerPrefs.GetFloat("VolumeSFX", 0.5f);
    }

    public void SetFpsTarget(int i)
    {
        switch(i)
        {
            case 0:
                Application.targetFrameRate = 60;
                break;
            case 1:
                Application.targetFrameRate = 120;
                break;
            case 2:
                Application.targetFrameRate = 150;
                break;
            case 3:
                Application.targetFrameRate = 300;              
                break;
        }
        PlayerPrefs.SetInt("FpsTarget", i);
    }

    public void OpenButtonPanel()
    {
        _panelButton.SetActive(true);
        _panelAudio.SetActive(false);
        _panelControl.SetActive(false);
        _panelGraphics.SetActive(false);
        eventSystem.SetSelectedGameObject(_firstButtonOptionMenu);
    }

    public void OpenPanelAudio()
    {
        HudControllerInGame.Instance.StateMenu = ActualMenu.InSettings;
        _panelAudio.SetActive(true);
        _panelButton.SetActive(false);
        eventSystem.SetSelectedGameObject(_firstBoutonPanelAudio);
    }

    public void OpenGraphicsPanel()
    {
        HudControllerInGame.Instance.StateMenu = ActualMenu.InSettings;
        _panelGraphics.SetActive(true);
        _panelButton.SetActive(false);
        eventSystem.SetSelectedGameObject(_firstBoutonPanelGraphics);
    }
    public void OpenControlPanel()
    {
        HudControllerInGame.Instance.StateMenu = ActualMenu.InSettings;
        _panelControl.SetActive(true);
        _panelButton.SetActive(false);
        eventSystem.SetSelectedGameObject(_firstBoutonPanelControl);
    }



    private void InitializeSensibility()
    {
        sliderSensibilityMouseX.value = PlayerPrefs.GetFloat("SensibilityMouseX", 100f);
        sliderSensibilityMouseY.value = PlayerPrefs.GetFloat("SensibilityGamePadY", 100f);
    }

    public void ChangeValueSliderAudio()
    {
        PlayerPrefs.SetFloat("VolumeMusic", sliderVolumeMusic.value);
        PlayerPrefs.SetFloat("Sound", sliderVolumeSoundEffect.value);
        Debug.Log(PlayerPrefs.GetFloat("Sound"));
        AudioManager.instance.ChangeVolumeMusic(sliderVolumeMusic.value);
        AudioManager.instance.ChangeVolumeSoundEFFect(sliderVolumeSoundEffect.value);
    }
    void InitializeAudioOption()
    {
        sliderVolumeMusic.value = PlayerPrefs.GetFloat("VolumeMusic", 0.5f);
        sliderVolumeSoundEffect.value = PlayerPrefs.GetFloat("Sound", 0.5f);
        AudioManager.instance.ChangeVolumeMusic(PlayerPrefs.GetFloat("VolumeMusic", 0.5f));
        AudioManager.instance.ChangeVolumeSoundEFFect(PlayerPrefs.GetFloat("Sound", 0.5f));
    }
    void InitializeGraphicsOptions()
    {
        Resolution[] resolution = Screen.resolutions;
        Array.Reverse(resolution);
        storeResolution = resolution;
        ScreenInitialize();
        AddResolution(resolution);
        ResolutionInitialize(storeResolution);


        //Initialize Data Save
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
        _displayTypeDropDown.value = PlayerPrefs.GetInt("ScreenMode");
        SetScreenMode();
        _vSyncToggle.isOn = (PlayerPrefs.GetInt("VSync") != 0);
        SetVSync();
        _antiAliasingDropDown.value = PlayerPrefs.GetInt("AntiAliasing");
        SetAntiAliasing();
        _resolutionDropDown.value = PlayerPrefs.GetInt("Resolution");
        SetResolution();
        _toggleShowFps.isOn = (PlayerPrefs.GetInt("ShowFps") != 0);
        ShowFps();
        _toggleCameraShake.isOn = (PlayerPrefs.GetInt("CameraShake") != 0);
        SetCameraShake();
        _toggleRumbler.isOn = (PlayerPrefs.GetInt("Rumbler") != 0);
        SetRumbler();
        _toggleCameraClamp.isOn = (PlayerPrefs.GetInt("CameraClamp") != 0);
        SetCameraClamp();

        SetFpsTarget( PlayerPrefs.GetInt("FpsTarget", 1));
        _dropDownFpsTarget.value = PlayerPrefs.GetInt("FpsTarget", 1);

    }

    public void SetScreenMode()
    {
        ScreenOptions(_displayTypeDropDown.value);

        PlayerPrefs.SetInt("ScreenMode", _displayTypeDropDown.value); 
    }

    public void ShowFps()
    {
        if (_toggleShowFps.isOn == false)
        {
            HudControllerInGame.Instance.SetShowFps(false);
        }
        else
        {
            HudControllerInGame.Instance.SetShowFps(true);
        }

        PlayerPrefs.SetInt("ShowFps", (_vSyncToggle.isOn ? 1 : 0));
    }

    public void SetVSync()
    {
        if (_vSyncToggle.isOn == false)
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
        }

        PlayerPrefs.SetInt("VSync", (_vSyncToggle.isOn ? 1 : 0));
    }

    public void SetCameraShake()
    {
        if (_toggleCameraShake.isOn == false)
        {
            UseCameraShake = false;
        }
        else
        {
            UseCameraShake = true;
        }

        PlayerPrefs.SetInt("CameraShake", (_vSyncToggle.isOn ? 1 : 0));
    }

    public void SetCameraClamp()
    {
        if (_toggleCameraClamp.isOn == false)
        {
            UseClampCamera = false;
        }
        else
        {
            UseClampCamera = true;
        }

        PlayerPrefs.SetInt("CameraClamp", (_vSyncToggle.isOn ? 1 : 0));
    }

    public void SetRumbler()
    {
        if (_toggleRumbler.isOn == false)
        {
            UseRumbler = false;
        }
        else
        {
            UseRumbler = true;
        }

        PlayerPrefs.SetInt("Rumbler", (_vSyncToggle.isOn ? 1 : 0));
    }

    public void SetAntiAliasing()
    {
        QualitySettings.antiAliasing = (int)MathF.Pow(2f, _antiAliasingDropDown.value);
        PlayerPrefs.SetInt("AntiAliasing", _antiAliasingDropDown.value);
    }

    public void SetResolution()
    {
        Screen.SetResolution(storeResolution[_resolutionDropDown.value].width, storeResolution[_resolutionDropDown.value].height, FullScreenMode.FullScreenWindow);
        PlayerPrefs.SetInt("Resolution", _resolutionDropDown.value);
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

    void ScreenInitialize()
    {
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            _displayTypeDropDown.value = 0;
            screenMode = FullScreenMode.ExclusiveFullScreen;
        }

        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            _displayTypeDropDown.value = 1;
            screenMode = FullScreenMode.Windowed;
        }

        else
        {
            _displayTypeDropDown.value = 2;
            screenMode = FullScreenMode.FullScreenWindow;

        }

        _displayTypeDropDown.RefreshShownValue();
    }


    void AddResolution(Resolution[] res)
    {
        countRes = 0;
        for (int i = 0; i < res.Length; i++)
        {
            if (res[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                storeResolution[countRes] = res[i];
                countRes++;
            }
        }

        for (int i = 0; i < countRes; i++)
        {
            _resolutionDropDown.options.Add(new TMP_Dropdown.OptionData(ResolutionToStrings(storeResolution[i])));
        }
    }

    private string ResolutionToStrings(Resolution resolution)
    {
        return resolution.width + " x " + resolution.height;
    }



    void ScreenOptions(int mode)
    {
        if (mode == 0)
        {
            screenMode = FullScreenMode.ExclusiveFullScreen;

        }

        else if (mode == 1)
        {

            screenMode = FullScreenMode.Windowed;
        }

        else
        {
            screenMode = FullScreenMode.FullScreenWindow;
        }

        Screen.fullScreenMode = screenMode;
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        PlayerPrefs.SetInt("QualityLevel", quality); ;
    }

}
