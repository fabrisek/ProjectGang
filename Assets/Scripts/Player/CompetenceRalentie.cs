using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CompetenceRalentie : MonoBehaviour
{
    [SerializeField] float _timeMax;
    [SerializeField] float _timeRestant;
    [SerializeField] GlitchEffect glitch;
    [SerializeField] bool competenceIsActive;


    public void ActiveSlowTime(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            ActiveSkill();
        }

        if (callback.canceled)
        {
            DesactiveSkill();
        }
    }

    private void DesactiveSkill()
    {
        Time.timeScale = 1f;
        glitch.enabled = false;
        AudioManager.instance.ChangePitch(1f);
        AudioManager.instance.playSoundEffect(3);
        competenceIsActive = false;
    }

    private void ActiveSkill()
    {
        competenceIsActive = true;
        Time.timeScale = 0.6f;
        AudioManager.instance.ChangePitch(.98f);
        glitch.enabled = true;
        AudioManager.instance.playSoundEffect(2);
    }

    private void Update()
    {
        if (competenceIsActive)
        {

            if (_timeRestant > 0)
            {
                _timeRestant -= Time.unscaledDeltaTime;
            }

            else
            {
                DesactiveSkill();
            }

            HudControllerInGame.Instance.ChangeSliderTimeValue(_timeRestant, _timeMax, true);
        }

        else
        {
            if (_timeRestant < _timeMax)
            {
                _timeRestant += Time.unscaledDeltaTime / 2;
                HudControllerInGame.Instance.ChangeSliderTimeValue(_timeRestant, _timeMax, true);
            }

            else
            {
                HudControllerInGame.Instance.ChangeSliderTimeValue(_timeRestant, _timeMax, false);
            }
        }
    }
}
