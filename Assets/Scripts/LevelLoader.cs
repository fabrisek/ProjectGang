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
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)
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
                TransitionScript.Instance.Fade(1f);
            }
            LoadSave.oldSceneLoad = LoadSave.sceneToLoad;
            LoadSave.first = true;
            
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            
                StartCoroutine(LoadAsyncTst());
            
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
        _loadingScreenPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            _loadingSlider.value = progress;
            _textSlider.text = progress * 100f + "%";
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator LoadAsyncTst()
    {
   
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.1f);
        SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
      
    }


    }
