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


    public bool IsGamePad { get; set; }
    public Transform camLookAt;

    private void Awake()
    {
        Instance = this;   
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraShakerHandler.Shake(constantShake);
        transform.LookAt(camLookAt);
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
        Vector2 looking = InputManager.Instance.GetPlayerLook();

        if(IsGamePad == false)
        {
            lookX = looking.x * Settings.SensibilityMouse * Time.unscaledDeltaTime;
            lookY = looking.y * Settings.SensibilityMouse * Time.unscaledDeltaTime;
        }
        else
        {
            lookX = looking.x * Settings.SensibilityGamePad * Time.unscaledDeltaTime;
            lookY = looking.y * Settings.SensibilityGamePad * Time.unscaledDeltaTime;
        }


        yRotation += lookX;
        xRotation -= lookY;
        //if(HUD_Settings.Instance.UseClampCamera)
        //{
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //}

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        Debug.Log(zTilt);
    }
}