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

    //bool corouteStart;
    private void Awake()
    {
        StartCoroutine(CoroutineStart());
    }

    IEnumerator CoroutineStart ()
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
        
       
        if (sceneIndex !=0 &&  LoadSave.sceneToLoad != LoadSave.oldSceneLoad)
        {
            if (TransitionScript.Instance != null)
            {
                // StopCoroutine(TransitionScript.Instance.CoroutineFadeV2());
                TransitionScript.Instance.Fade(1f);
            }
            LoadSave.oldSceneLoad = LoadSave.sceneToLoad;
            LoadSave.first = true;


            //SceneManager.LoadScene(1);
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            
                StartCoroutine(LoadAsyncTst());
            
        }
        else
        {
            if (TransitionScript.Instance != null)
            {
                // StopCoroutine(TransitionScript.Instance.CoroutineFadeV2());
                TransitionScript.Instance.Fade(2f);
            }
            LoadSave.first = false;
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }

    }

    

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        _loadingScreenPanel.SetActive(true);
        //operation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        // AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
       // SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            _loadingSlider.value = progress;
            _textSlider.text = progress * 100f + "%";
            yield return new WaitForSeconds(0.01f);
            //_loadingScreenPanel.SetActive(false);


        }
       // SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
        /* Debug.Log("je me unload + 1");
        SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
        Debug.Log("ahah cheh");
        StartCoroutine(LoadAsyncTst());*/
    }
    IEnumerator LoadAsyncTst()
    {
       
        yield return new WaitForSeconds(0.1f);
        //Time.timeScale = 1;
        Debug.Log("je me unload");
        SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
        Debug.Log("ahah cheh");

    }


    }
