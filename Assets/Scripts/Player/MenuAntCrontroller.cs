using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuAntCrontroller : MonoBehaviour
{
    Input _input;
    InputAction _movement;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 8.0f;
    [SerializeField] float speedRotation;

    LevelSelectorMenu _levelRef;

    public void SetLevelRef(LevelSelectorMenu levelref) { _levelRef = levelref; }
    // Start is called before the first frame update

    private void Awake()
    {
        _input = InputManager._input;
        _input.InMainMenu.Select.performed += SelectMenu;
        _input.InMainMenu.Back.performed += BackAction;
    }

    private void OnEnable()
    {
        _input.InMainMenu.Enable();
        _movement = _input.InMainMenu.Move;   
    }

    private void OnDisable()
    {
        _input.InMainMenu.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = _input.InMainMenu.Move.ReadValue<Vector2>();
        Vector3 move = (transform.forward * movementInput.y + transform.right * movementInput.x);
        
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);
        RotatePlayer(movementInput);
    }

    void RotatePlayer (Vector3 move)
    {
        if (move.x != 0)
        {
            transform.Rotate(Vector3.up * move.x * speedRotation * Time.deltaTime);
        }
        
        

      
    }

    void SelectMenu(InputAction.CallbackContext callback)
    {
        if (_levelRef != null)
        {
            LevelLoader.Instance.LoadLevel(_levelRef.GetSceneNumber());
        }
    }

    void BackAction(InputAction.CallbackContext callback)
    {
        HUD_MainMenu.Instance.CloseSettings();
    }
}
