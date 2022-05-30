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
        inputActions.InGame.Restart.performed += RestartLevel;
        inputActions.InGame.Restart.canceled -= RestartLevel;
        CutMovePlayer();
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
        StartCoroutine(CoroutineTroisDeuxUn());
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
            LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator CoroutineTroisDeuxUn ()
    {
        yield return new WaitForSeconds(3);
        ResetMovePlayer();
        startTimer = true;


    }




}
