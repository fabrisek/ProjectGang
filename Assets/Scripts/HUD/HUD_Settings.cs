using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;
using Doozy.Runtime.UIManager.Components;

public class HUD_Settings : MonoBehaviour
{
    public static HUD_Settings Instance;

    [SerializeField] GameObject _panelAudio;
    [SerializeField] GameObject _panelControl;
    [SerializeField] GameObject _panelGraphics;
    [SerializeField] GameObject _panelButton;

    [SerializeField] TMP_Dropdown _resolutionDropDown;
    [SerializeField] TMP_Dropdown _displayTypeDropDown;
    [SerializeField] TMP_Dropdown _antiAliasingDropDown;

    [SerializeField] UIToggle _vSyncToggle;
    [SerializeField] Slider sliderSensibilityMouseX;
    [SerializeField] Slider sliderSensibilityMouseY;

    [SerializeField] Slider sliderVolumeSoundEffect;
    [SerializeField] Slider sliderVolumeMusic;
    FullScreenMode screenMode;
    private Resolution[] storeResolution;
    int countRes;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InitializeGraphicsOptions();
        InitializeAudioOption();
        InitializeSensibility();
    }

    public void OpenButtonPanel()
    {
        _panelButton.SetActive(true);
        _panelAudio.SetActive(false);
        _panelControl.SetActive(false);
        _panelGraphics.SetActive(false);
    }

    public void OpenPanelAudio()
    {
        HudControllerInGame.Instance.StateMenu = ActualMenu.InSettings;
        _panelAudio.SetActive(true);
        _panelButton.SetActive(false);
    }

    public void OpenGraphicsPanel()
    {
        HudControllerInGame.Instance.StateMenu = ActualMenu.InSettings;
        _panelGraphics.SetActive(true);
        _panelButton.SetActive(false);
    }
    public void OpenControlPanel()
    {
        HudControllerInGame.Instance.StateMenu = ActualMenu.InSettings;
        _panelControl.SetActive(true);
        _panelButton.SetActive(false);
    }

    private void InitializeSensibility()
    {
        sliderSensibilityMouseX.value = PlayerPrefs.GetFloat("SensibilityMouseX", 200);
        sliderSensibilityMouseY.value = PlayerPrefs.GetFloat("SensibilityMouseY", 200);
        PlayerPrefs.SetFloat("SensibilityMouseX", 200);
        PlayerPrefs.SetFloat("SensibilityMouseY", 200);
        PlayerPrefs.SetFloat("SensibilityGamePadX", 200);
        PlayerPrefs.SetFloat("SensibilityGamePadY", 200);
    }

    public void ChangeValueSliderAudio()
    {
        PlayerPrefs.SetFloat("VolumeMusic", sliderVolumeMusic.value);
        PlayerPrefs.SetFloat("VolumeSFX", sliderVolumeSoundEffect.value);
        AudioManager.instance.ChangeVolumeMusic(sliderVolumeMusic.value);
        AudioManager.instance.ChangeVolumeSoundEFFect(sliderVolumeSoundEffect.value);
    }
    public void ChangeValueMouseSensibilitySlider(float x, float y)
    {
        sliderSensibilityMouseX.value = x;
        sliderSensibilityMouseY.value = y;
        PlayerPrefs.SetFloat("SensibilityMouseX", x); ;
        PlayerPrefs.SetFloat("SensibilityMouseY", y); ;
    }
    void InitializeAudioOption()
    {
        sliderVolumeMusic.value = PlayerPrefs.GetFloat("VolumeMusic", 0.5f);
        sliderVolumeSoundEffect.value = PlayerPrefs.GetFloat("VolumeSFX", 0.5f);
        AudioManager.instance.ChangeVolumeMusic(PlayerPrefs.GetFloat("VolumeMusic", 0.5f));
        AudioManager.instance.ChangeVolumeSoundEFFect(PlayerPrefs.GetFloat("VolumeSFX", 0.5f));
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
    }

    public void SetScreenMode()
    {
        ScreenOptions(_displayTypeDropDown.value);

        PlayerPrefs.SetInt("ScreenMode", _displayTypeDropDown.value); 
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
