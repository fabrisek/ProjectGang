using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] GroupesBugsManager groupesBugsManager;
    [SerializeField] PlayerMovementAdvanced playerMovementScript;
    [SerializeField] WallRunningAdvanced wallRunScript;
    [SerializeField] CompetenceRalentie slowDown;
    [SerializeField] PlayerCam playercam;
    [SerializeField] float timeOfThreeTwoOneGo;

    bool startTimer;
    // Start is called before the first frame update
    void Awake()
    {
        SetTimeTOBugsManager();
        Instance = this;
    }

    private void Start()
    {
        if(timeOfThreeTwoOneGo == 0)
        {
            timeOfThreeTwoOneGo = 2;
        }
        CutMovePlayer();
        StartCoroutine(CoroutineTroisDeuxUn());
        HudControllerInGame.Instance.StartThreeTwoOne(timeOfThreeTwoOneGo);
        
    }

    void CutMovePlayer()
    {
        InputManager._input.InGame.Disable();
        playerMovementScript.enabled = false;
        wallRunScript.enabled = false;
        slowDown.enabled = false;
        playercam.enabled = false;
    }

    void ResetMovePlayer()
    {
        InputManager._input.Enable();
        playerMovementScript.enabled = true;
        wallRunScript.enabled = true;
        slowDown.enabled = true;
        playercam.enabled = true;
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
    public void RestartLevel()
    {

            if (HudControllerInGame.Instance.InMenu == false)
        {
            Time.timeScale = 1;
                LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }

            else
            {
                HudControllerInGame.Instance.Back();
            }
        
    }

    IEnumerator CoroutineTroisDeuxUn ()
    {
        yield return new WaitForSeconds(timeOfThreeTwoOneGo);
        ResetMovePlayer();
        startTimer = true;


    }




}
