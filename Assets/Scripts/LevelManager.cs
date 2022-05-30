using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class LevelManager : MonoBehaviour
{
    [SerializeField] GroupesBugsManager groupesBugsManager;
    [SerializeField] PlayerMovementAdvanced playerMovementScript;

    public Input inputActions;
    // Start is called before the first frame update
    void Awake()
    {
        //Inputs
        inputActions = new Input();
        inputActions.InGame.Restart.performed += RestartLevel;
        inputActions.InGame.Restart.canceled -= RestartLevel;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        
        LauchTimer();
        SetTimeTOBugsManager();

    }

    void SetTimeTOBugsManager ()
    {
        if(groupesBugsManager != null)
        {
            groupesBugsManager.Time = Timer.Instance.GetTimer();
        }
    }

    void LauchTimer ()
    {
        if (playerMovementScript != null)
        {
            if (!Timer.Instance.TimerIsLaunch() && playerMovementScript.GetInputActivated)
            {
                Timer.Instance.LaunchTimer();
            }
        }
    }
    void RestartLevel(InputAction.CallbackContext callback)
    {
        if(callback.performed)
        {
            LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }




}
