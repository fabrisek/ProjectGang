using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    [SerializeField] float timeMinLoadScrean;
    [SerializeField] float timeWaitMore;
    [SerializeField] int sceneindex;
   
    int sceneToLoad;
    bool minTimeDone;
    bool canLoad;
    bool start;
    AsyncOperation operation;
    // Start is called before the first frame update
    void Start()
    {
        if (TransitionScript.Instance != null)
        {
            TransitionScript.Instance.CoroutineFade(false);
        }
        sceneToLoad = LoadSave.sceneToLoad;
        StartCoroutine(MinTimeTODo());
        operation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(operation.isDone)
        {
            if(!start)
            {
               
                StartCoroutine(CanLoadTODo());
            }
            
            if (minTimeDone && canLoad)
            {
                if (LevelManager.Instance != null)
                {
                    //  LevelManager.Instance.InitLevelManager();
                    if (CinematicController.Instance != null)
                    {
                       
                        CinematicController.Instance.SetStartCinematci();
                    }
                    else
                    {
                        
                        LevelManager.Instance.InitLevelManager();
                    }
                }
                SceneManager.UnloadScene(sceneindex);

            }
        }
    }

    IEnumerator MinTimeTODo()
    {
        yield return new WaitForSeconds(timeMinLoadScrean);
        minTimeDone = true;
    }

    IEnumerator CanLoadTODo()
    {
        start = true;
        yield return new WaitForSeconds(timeMinLoadScrean);
        canLoad = true;

    }


}
