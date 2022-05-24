using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CompetenceRalentie : MonoBehaviour
{
    [SerializeField] GlitchEffect glitch;
    public void ActiveSlowTime(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            Time.timeScale = 0.6f;
            AudioManager.instance.ChangePitch(.98f);
            glitch.enabled = true;
        }

        if (callback.canceled)
        {
            Time.timeScale = 1f;
            glitch.enabled = false;
            AudioManager.instance.ChangePitch(1f);
        }
    }
}
