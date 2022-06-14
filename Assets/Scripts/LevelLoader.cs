using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [SerializeField] GameObject _loadingScreenPanel;
    [SerializeField] Slider _loadingSlider;
    [SerializeField] TextMeshProUGUI _textSlider;
    [SerializeField] float tempsLoading = 10;

    bool canLoad;
    //bool corouteStart;
    private void Awake()
    {
       
        StartCoroutine(CoroutineStart());
    }

    IEnumerator CoroutineStart()
    {
        yield return new WaitForSeconds(1);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        }
        else
        {
            Instance = this;
        }
    }



    public void LoadLevel(int sceneIndex)
    {
        if (Data_Manager.Instance != null)
            Data_Manager.AlreadyInGame = true;
        AudioManager.instance.StopMusic();
        LoadSave.sceneToLoad = sceneIndex;


        if (sceneIndex != 0 && LoadSave.sceneToLoad != LoadSave.oldSceneLoad)
        {
            Time.timeScale = 1;
           
            LoadSave.oldSceneLoad = LoadSave.sceneToLoad;
            LoadSave.first = true;



            StartCoroutine(LoadAsyncTst(sceneIndex));

        }
        else
        {
            if (TransitionScript.Instance != null)
            {

                TransitionScript.Instance.Fade(2f);
            }
            LoadSave.first = false;
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }

    }



    IEnumerator LoadAsynchronously(int sceneIndex)
    {
       
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return new WaitForSeconds(0.01f);
        
    }
    IEnumerator LoadAsyncTst(int sceneIndex)
    {
        //AfficherCanvas
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        
        
        StartCoroutine(WaitCoroutine(tempsLoading));
        while (!operation.isDone)
        {
            //Mettre tips et progresse
            if (operation.progress >= 0.9f)
            {
                if(canLoad)
                {
                    if (TransitionScript.Instance != null)
                    {
                        
                        TransitionScript.Instance.Fade(1f);
                    }
                    operation.allowSceneActivation = true;
                    
                }


            }
            yield return null;
        }



    }

    IEnumerator WaitCoroutine (float time)
    {
        canLoad = false;
        yield return new WaitForSeconds(time);
        
        canLoad = true;
    }
}
