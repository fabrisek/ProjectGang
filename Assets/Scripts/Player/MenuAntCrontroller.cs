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
    [SerializeField] float speedRotationHead;
    [SerializeField] GameObject view;
    [SerializeField] GameObject head;
    [SerializeField] GameObject ass;

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
        if (view != null)
        {
            RotatePlayer(move);
            if(head != null)
            {
                RotateHead(move);
            }

            if (ass != null)
            {
                RotateAss(move);
            }
        }
    }

    void RotatePlayer (Vector3 move)
    {
        if (move != Vector3.zero)
        {
           
            Quaternion rotationRef = Quaternion.LookRotation(move.normalized);
           
            view.transform.rotation = Quaternion.RotateTowards(view.transform.rotation, rotationRef, speedRotation * Time.deltaTime);
        }
      
    }

    void RotateHead(Vector3 move)
    {

        if (move != Vector3.zero)
        {
                Quaternion rotationRef = Quaternion.LookRotation(move.normalized);
                Quaternion temp = Quaternion.RotateTowards(head.transform.rotation, rotationRef, speedRotationHead * Time.deltaTime);
            if (temp.eulerAngles.y - view.transform.eulerAngles.y <= 45 && temp.eulerAngles.y - view.transform.eulerAngles.y >= -45)
            {
                head.transform.rotation = temp;
            }
        }
        else
        {
            head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation,view.transform.rotation, speedRotationHead * Time.deltaTime);
        }

    }

    void RotateAss(Vector3 move)
    {
      
        if (move != Vector3.zero)
        {   
                Quaternion rotationRef = Quaternion.LookRotation(move.normalized);
                Quaternion temp = Quaternion.RotateTowards(ass.transform.rotation, rotationRef, speedRotationHead * Time.deltaTime);
            if (temp.eulerAngles.y - view.transform.eulerAngles.y <= 25 && temp.eulerAngles.y - view.transform.eulerAngles.y >= -20)
            {
                ass.transform.rotation = temp;
            }
        }
        else
        {
            ass.transform.rotation = Quaternion.RotateTowards(ass.transform.rotation, view.transform.rotation, speedRotationHead * Time.deltaTime);
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
