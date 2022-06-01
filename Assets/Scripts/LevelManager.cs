using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class LevelManager : MonoBehaviour
{
    [SerializeField] GroupesBugsManager groupesBugsManager;
    [SerializeField] PlayerMovementAdvanced playerMovementScript;
    [SerializeField] WallRunningAdvanced wallRunScript;
    [SerializeField] CompetenceRalentie slowDown;

    public Input inputActions;
    bool startTimer;
    // Start is called before the first frame update
    void Awake()
    {
        //Inputs
        inputActions = new Input();
        inputActions.InGame.RestartAndBack.performed += RestartLevel;
        inputActions.InGame.RestartAndBack.canceled -= RestartLevel;
        SetTimeTOBugsManager();

    }

    private void Start()
    {
        CutMovePlayer();
        StartCoroutine(CoroutineTroisDeuxUn());
        HudControllerInGame.Instance.StartThreeTwoOne(3);
        
    }

    void CutMovePlayer()
    {
        inputActions.InGame.Disable();
        playerMovementScript.enabled = false;
        wallRunScript.enabled = false;
        slowDown.enabled = false;
    }

    void ResetMovePlayer()
    {
        inputActions.Enable();
        playerMovementScript.enabled = true;
        wallRunScript.enabled = true;
        slowDown.enabled = true;
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
        //Debug.Log(inputActions.InGame.Move.enabled);
        LauchTimer();
       

    }

    void SetTimeTOBugsManager ()
    {
        if(groupesBugsManager != null)
        {
            groupesBugsManager.Player = playerMovementScript.transform;
        }
    }

    void LauchTimer ()
    {
        if (playerMovementScript != null)
        {
            if (!Timer.Instance.TimerIsLaunch() && startTimer)
            {
                Timer.Instance.LaunchTimer();
            }
        }
    }
    void RestartLevel(InputAction.CallbackContext callback)
    {
        if(callback.performed)
        {
            if (HudControllerInGame.Instance.InMenu == false)
                LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);

            else
            {
                HudControllerInGame.Instance.Back();
            }
        }
    }

    IEnumerator CoroutineTroisDeuxUn ()
    {
        yield return new WaitForSeconds(3);
        ResetMovePlayer();
        startTimer = true;


    }




}
