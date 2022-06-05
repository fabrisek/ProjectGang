using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScript : MonoBehaviour
{
    [SerializeField] float timeMinLoadScrean;
    [SerializeField] float timeWaitMore;
    [SerializeField] int sceneindex;
    [SerializeField] Slider slider;
    [SerializeField] string[] tips;
    [SerializeField] TextMeshProUGUI textTips;
    [SerializeField] TextMeshProUGUI sliderPercentText;
    [SerializeField] TextMeshProUGUI NameLevel;
    [SerializeField] Image background;
    int sceneToLoad;
    bool minTimeDone;
    bool canLoad;
    bool start;
    AsyncOperation operation;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0;
        sceneToLoad = LoadSave.sceneToLoad;
        StartCoroutine(MinTimeTODo());
        operation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        slider.maxValue = timeMinLoadScrean;
        int rand = Random.Range(0, tips.Length);
        textTips.text = "TIPS : " + tips[rand];

        if (Data_Manager.Instance != null)
        {
            DATA data = Data_Manager.Instance.GetData();
            for(int i = 0; i < data._worldData.Count; i++)
            {
                for (int j = 0; j < data._worldData[i]._mapData.Count; j++)
                {
                    if (data._worldData[i]._mapData[j].GetSceneData().IndexScene == sceneToLoad)
                    {
                        background.sprite = data._worldData[i]._mapData[j].GetSceneData().BackGroundLoad;
                        NameLevel.text = data._worldData[i]._mapData[j].GetSceneData().MapName;
                        return;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        sliderPercentText.text = ((int)(slider.normalizedValue * 100)).ToString() + " %";
        if (operation.isDone)
        {
            if(!start)
            {               
                StartCoroutine(CanLoadTODo());
            }
            slider.value += Time.unscaledDeltaTime;
            if (minTimeDone && canLoad)
            {
                if (TransitionScript.Instance != null)
                {
                    //  StopCoroutine(TransitionScript.Instance.CoroutineFadeV2());
                    TransitionScript.Instance.Fade();
                }
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
               
                SceneManager.UnloadSceneAsync(sceneindex);

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
