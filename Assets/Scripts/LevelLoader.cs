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
    [SerializeField] float tempsLoading;

    [SerializeField] string[] tips;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI textTips;
    [SerializeField] TextMeshProUGUI sliderPercentText;
    [SerializeField] TextMeshProUGUI NameLevel;
    [SerializeField] GameObject Canavas;

    bool canLoad;
    bool loading;
    float timeLeft;
    //bool corouteStart;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        }
        else
        {
            Instance = this;
        }
        canLoad = true;
        loading = false;
        //StartCoroutine(CoroutineStart());
    }
    private void Start()
    {
        timeLeft = tempsLoading;
    }
    private void Update()
    {
        if (!canLoad)
        {
            timeLeft -= Time.unscaledDeltaTime;
            if (timeLeft < 0)
            {
                canLoad = true;
                timeLeft = tempsLoading;

            }
        }
    }
    IEnumerator CoroutineStart()
    {
        yield return new WaitForSeconds(1);
       
    }



    public void LoadLevel(int sceneIndex)
    {
        if (Data_Manager.Instance != null)
            Data_Manager.AlreadyInGame = true;
        //AudioManager.instance.StopMusic();
        //Debug.Log("yo");
        LoadSave.sceneToLoad = sceneIndex;

     //   AudioManager.instance.PlayMusic(3);
        if (sceneIndex != 0 && LoadSave.sceneToLoad != LoadSave.oldSceneLoad)
        {
            AudioManager.instance.PlayMusic(3);
            Time.timeScale = 1;
           
            LoadSave.oldSceneLoad = LoadSave.sceneToLoad;
            LoadSave.first = true;


            //Debug.Log("yo");
            if (TransitionScript.Instance != null)
            {

                TransitionScript.Instance.Fade(0.5f);
            }
            ShowLoadingCanvas(sceneIndex);
            if (canLoad)
            {
                canLoad = false;
                /*StopCoroutine(WaitCoroutine(tempsLoading));
                StartCoroutine(WaitCoroutine(tempsLoading));*/
            }
            if (!loading)
            {
                StopCoroutine(LoadAsyncTst(sceneIndex));
                StartCoroutine(LoadAsyncTst(sceneIndex));
            }

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
        loading = true;
        //AfficherCanvas
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        operation.allowSceneActivation = false;

       // Debug.Log("Senece a Load" + sceneIndex);

       // StartCoroutine(WaitCoroutine(tempsLoading));
        while (!operation.isDone)
        {
            //Mettre tips et progresse
            slider.value +=Time.unscaledDeltaTime;
            sliderPercentText.text = ((int)(slider.normalizedValue * 100)).ToString() + " %";
            if (operation.progress >= 0.9f)
            {
               // Debug.Log("Operation a plus de 0.9");
                if (slider.normalizedValue * 100 >= 100)
                {
                   // Debug.Log("je peut me load");
                    if (TransitionScript.Instance != null)
                    {
                        
                        TransitionScript.Instance.Fade(1f);
                    }
                    operation.allowSceneActivation = true;
                    AudioManager.instance.StopMusic();

                }


            }
            yield return null;
        }



    }

    IEnumerator WaitCoroutine (float time)
    {
        canLoad = false;
     //   Debug.Log("je start le temps"+ time);
       // Debug.Log("je start le temps");
        yield return new WaitForSeconds(time);
     //   Debug.Log("je finie tempss");
        canLoad = true;
    }

    void ShowLoadingCanvas (int sceneIndex)
    {
        Canavas.SetActive(true);
        slider.value = 0;
        slider.maxValue = tempsLoading;
        int rand = Random.Range(0, tips.Length);
        textTips.text = "TIPS : " + tips[rand];
        if (Data_Manager.Instance != null)
        {
            DATA data = Data_Manager.Instance.GetData();
            for (int i = 0; i < data._worldData.Count; i++)
            {
                for (int j = 0; j < data._worldData[i]._mapData.Count; j++)
                {
                    if (data._worldData[i]._mapData[j].GetSceneData().IndexScene == sceneIndex)
                    {
                      //  Debug.Log("Je passe voir les data");
                        //background.sprite = data._worldData[i]._mapData[j].GetSceneData().BackGroundLoad;
                        NameLevel.text = data._worldData[i]._mapData[j].GetSceneData().MapName;
                        return;
                    }
                }
            }
        }
        
    }
}
