using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class KeyBindUi : MonoBehaviour
{
    [SerializeField] InputActionReference _inputActionReference;
    [SerializeField] bool _excludeMouse = true;
    [Range(0,10)]
    [SerializeField] int _selectedBinding;
    [SerializeField] InputBinding.DisplayStringOptions _displayStringOptions;

    [SerializeField] InputBinding _inputBinding;
    private int _bindingIndex;

    private string _actionName;


    [Header("UI FIELD")]
    [SerializeField] TextMeshProUGUI _actionText;
    [SerializeField] Button _rebindButton;
    [SerializeField] TextMeshProUGUI _rebindText;
    [SerializeField] Button _resetButton;

    private void OnEnable()
    {
        _rebindButton.onClick.AddListener(() => DoRebind());
        _resetButton.onClick.AddListener(() => ResetBinding());

        if (_inputActionReference != null)
        {
            GetBindingInfo();
            UpdateUI();
        }

        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
    }

    public void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }

    private void OnValidate()
    {
        GetBindingInfo();
        UpdateUI();
    }

    void GetBindingInfo()
    {
        if (_inputActionReference.action != null)
        { _actionName = _inputActionReference.action.name; }

        if (_inputActionReference.action.bindings.Count > _selectedBinding)
        {
            _inputBinding = _inputActionReference.action.bindings[_selectedBinding];
            _bindingIndex = _selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if (_actionText != null)
        {
            _actionText.text = _actionName;
        }

        if (_rebindText != null)
        {
            if (Application.isPlaying)
            {
                _rebindText.text = InputManager.GetBindingName(_actionName, _bindingIndex);
            }
            else
            {
                _rebindText.text = _inputActionReference.action.GetBindingDisplayString(_bindingIndex);
            }
        }
    }

    private void DoRebind()
    {
        InputManager.StartRebind(_actionName, _bindingIndex, _rebindText, _excludeMouse);
    }

    private void ResetBinding()
    {

    }
}
