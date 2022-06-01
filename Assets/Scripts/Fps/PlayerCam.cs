using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam Instance;
    public float sensX ;
    public float sensY ;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;
    private Vector2 mousePosition;

    public ShakeData constantShake; 

     Input inputActions;

    public bool IsGamePad { get; set; }

    private void Awake()
    {
        Instance = this;
        inputActions = new Input();
        
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraShakerHandler.Shake(constantShake);
        transform.forward = Vector3.forward;
    }

    private void Update()
    {
        DoLooking();    
    }
    private void DoLooking()
    {
        //getMouse Inputs
        float lookX;
        float lookY;
        Vector2 looking = GetPlayerLook();
        if(IsGamePad == false)
        {
            lookX = looking.x * InputManager.SensibilityMouseX * Time.unscaledDeltaTime;
            lookY = looking.y * InputManager.SensibilityMouseY * Time.unscaledDeltaTime;
        }
        else
        {
            lookX = looking.x * InputManager.SensibilityGamePadX * Time.unscaledDeltaTime;
            lookY = looking.y * InputManager.SensibilityGamePadY * Time.unscaledDeltaTime;
        }


        yRotation += lookX;
        xRotation -= lookY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public Vector2 GetPlayerLook()
    {
        return inputActions.InGame.Look.ReadValue<Vector2>();
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}