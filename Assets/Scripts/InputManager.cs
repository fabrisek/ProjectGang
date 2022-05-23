using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class InputManager : MonoBehaviour
{
    public static Input _input;
    public static InputManager Instance;
    public static float SensibilityMouseY;
    public static float SensibilityMouseX;   
    public static float SensibilityGamePadY;
    public static float SensibilityGamePadX;

    public static event Action rebindComplete;
    public static event Action rebindCanceled;
    public static event Action<InputAction, int> rebindStarted;
    private void Awake()
    {
        
            if (Instance != null && Instance != this)
                Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

            Instance = this;
        DontDestroyOnLoad(this.gameObject);


        if (_input == null)
        {
            _input = new Input();
        }

        SensibilityMouseX = PlayerPrefs.GetFloat("SensibilityMouseX", 2); ;
        SensibilityMouseY = PlayerPrefs.GetFloat("SensibilityMouseY", 2); ;
        SensibilityGamePadX = PlayerPrefs.GetFloat("SensibilityGamePadX", 2); ;
        SensibilityGamePadY = PlayerPrefs.GetFloat("SensibilityGamePadY", 2); ;
    }

    public void SetSensibilityXMouse(float x)
    {
        PlayerPrefs.SetFloat("SensibilityMouseX", x);
        SensibilityMouseX = x;
    }

    public void SetSensibilityXGamePad(float x)
    {
        PlayerPrefs.SetFloat("SensibilityGamePadX", x);
        SensibilityGamePadX = x;
    }

    public void SetSensibilityYGamePad(float y)
    {
        PlayerPrefs.SetFloat("SensibilityGamePadY", y);
        SensibilityGamePadY = y;
    }

    public void SetSensibilityYMouse(float y)
    {
        PlayerPrefs.SetFloat("SensibilityMouseY", y);
        SensibilityMouseY = y;
    }

    public static void StartRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse)
    {
        InputAction action = _input.asset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couln't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
            {
                DoRebind(action, bindingIndex, statusText, true,excludeMouse);
            }
        }

        else
            DoRebind(action, bindingIndex, statusText, false,excludeMouse);
    }

    private static void DoRebind(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI statusText, bool allCompositeParts, bool excludeMouse)
    {
        if (actionToRebind == null || bindingIndex < 0)
            return;

        statusText.text = $"Press a {actionToRebind.expectedControlType}";
        actionToRebind.Disable();
        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);
        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
            }
            SaveBindingOverride(actionToRebind);
            rebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            rebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");

        if (excludeMouse)
            rebind.WithControlsExcluding("Mouse");

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
    }


    public static string GetBindingName( string actionName, int bindingIndex)
    {
        if (_input == null)
        {
            _input = new Input();
        }

        InputAction action = _input.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }

    private static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (_input == null)
            _input = new Input();

        InputAction action = _input.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i, action.bindings[i].overridePath)))
            {
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i, action.bindings[i].overridePath));
            }
        }
    }

    public static void ResetBinding(string actionName, int bindingIndex)
    {
        InputAction action = _input.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex)
        {
            print("Coulnd not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
            {
                action.RemoveBindingOverride(i);
            }
        }

        else
            action.RemoveBindingOverride(bindingIndex);



        SaveBindingOverride(action);
    }
}
