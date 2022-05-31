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
    bool startTimer;
    // Start is called before the first frame update
    void Awake()
    {
        //Inputs
        inputActions = new Input();
        inputActions.InGame.RestartAndBack.performed += RestartLevel;
        inputActions.InGame.RestartAndBack.canceled -= RestartLevel;
        CutMovePlayer();
    }

    private void Start()
    {
        StartCoroutine(CoroutineTroisDeuxUn());
        HudControllerInGame.Instance.StartThreeTwoOne(3);
    }

    void CutMovePlayer()
    {
        playerMovementScript.enabled = false;
    }

    void ResetMovePlayer()
    {
        playerMovementScript.enabled = true;
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
