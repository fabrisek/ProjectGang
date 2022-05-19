using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [SerializeField] HudControllerInGame _hud;
    [SerializeField] float _timer;
    bool _timerIsLaunch;
    public float GetTimer() { return _timer; }
    public bool TimerIsLaunch() { return _timerIsLaunch; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LaunchTimer();
    }
    public void LaunchTimer()
    {        
        _timerIsLaunch = true;
    }

    private void Update()
    {
        if (_timerIsLaunch)
        {
            _timer += Time.deltaTime;
            _hud.ChangeTimerHud(_timer);
        }
    }

    public void StopTimer()
    {
        _timerIsLaunch = false;
    }

    public static string FormatTime(float time)
    {
        int intTime = (int)time;
        int Minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = String.Format("{0:00}:{1:00}:{2:000}", Minutes, seconds, fraction);
        return timeText;
    }
}
