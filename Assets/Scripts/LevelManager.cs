using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] Camera playerCam;
    [SerializeField] GroupesBugsManager groupesBugsManager;
    [SerializeField] PlayerMovementAdvanced playerMovementScript;
    [SerializeField] WallRunningAdvanced wallRunScript;
    [SerializeField] CompetenceRalentie slowDown;
    [SerializeField] PlayerCam playercam;
    [SerializeField] float timeOfThreeTwoOneGo;
    [SerializeField] GameObject CanvasInGame;

    public bool firstTime;
    //bool startcine;
    // Start is called before the first frame update
    void Awake()
    {
        playerCam.enabled = false;
        SetTimeTOBugsManager();
        Instance = this;
        CutMovePlayer();
        CanvasInGame.SetActive(false);

    }

    private void Start()
    {

        if (LoadSave.first)
        {
           // playerCam.enabled = false;
            //lancement De la cinematique

        }
        else
        {
            enableCam();
            InitLevelManager();

        }
    }

    public void enableCam ()
    {
        playerCam.enabled = true;
    }

    public void InitLevelManager()
    {
        if (CinematicController.Instance == null)
        {
            enableCam();
        }
        

        CanvasInGame.SetActive(true);
        if (timeOfThreeTwoOneGo == 0)
        {
            timeOfThreeTwoOneGo = 2;
        }

        StartCoroutine(CoroutineTroisDeuxUn());
        HudControllerInGame.Instance.StartThreeTwoOne(timeOfThreeTwoOneGo);
        
    }

    void CutMovePlayer()
    {
        InputManager._input.InGame.Disable();
        playerMovementScript.enabled = false;
        wallRunScript.enabled = false;
        slowDown.enabled = false;
        //playercam.enabled = false;
    }

    void ResetMovePlayer()
    {
        InputManager._input.Enable();
        playerMovementScript.enabled = true;
        wallRunScript.enabled = true;
        slowDown.enabled = true;
      //  playercam.enabled = true;
    }

    void SetTimeTOBugsManager()
    {
        if (groupesBugsManager != null)
        {
            groupesBugsManager.Player = playerMovementScript.transform;
        }
    }

    void LauchTimer()
    {
        if (playerMovementScript != null)
        {
            if (!Timer.Instance.TimerIsLaunch())
            {
                Timer.Instance.LaunchTimer();
            }
        }
    }
    public void RestartLevel()
    {

        if (HudControllerInGame.Instance.InMenu == false)
            LevelLoader.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);

        else
        {
            HudControllerInGame.Instance.Back();
        }

    }

    IEnumerator CoroutineTroisDeuxUn()
    {
        yield return new WaitForSeconds(timeOfThreeTwoOneGo);
        ResetMovePlayer();
        LauchTimer();
    }

    
}
