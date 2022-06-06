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

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        Instance = this;     
    }

   


    public void LoadLevel(int sceneIndex)
    {
        if (Data_Manager.Instance != null)
            Data_Manager.AlreadyInGame = true;
        AudioManager.instance.StopMusic();
        LoadSave.sceneToLoad = sceneIndex;
        if (TransitionScript.Instance != null)
        {
            // StopCoroutine(TransitionScript.Instance.CoroutineFadeV2());
            TransitionScript.Instance.Fade(1f);
        }
        else
        {
            Debug.Log("l'instanceTransition existe pas mec");
        }
        if (sceneIndex !=0 &&  LoadSave.sceneToLoad != LoadSave.oldSceneLoad)
        {
            LoadSave.oldSceneLoad = LoadSave.sceneToLoad;
            LoadSave.first = true;
            
          
            SceneManager.LoadScene(1);
        }
        else
        {
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
            yield return new WaitForSeconds(100);
            //_loadingScreenPanel.SetActive(false);
            Time.timeScale = 1;

        }


    }
}
