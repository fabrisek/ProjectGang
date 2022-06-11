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

    bool canStart;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0;
        sceneToLoad = LoadSave.sceneToLoad;
        StartCoroutine(MinTimeTODo());
        StartCoroutine(CoroutineDeSecuriter());


        if (Data_Manager.Instance != null)
        {
            DATA data = Data_Manager.Instance.GetData();
            for(int i = 0; i < data._worldData.Count; i++)
            {
                for (int j = 0; j < data._worldData[i]._mapData.Count; j++)
                {
                    if (data._worldData[i]._mapData[j].GetSceneData().IndexScene == sceneToLoad)
                    {
                        //background.sprite = data._worldData[i]._mapData[j].GetSceneData().BackGroundLoad;
                        NameLevel.text = data._worldData[i]._mapData[j].GetSceneData().MapName;
                        return;
                    }
                }
            }
        }
    }

    IEnumerator CoroutineDeSecuriter ()
    {
        yield return new WaitForSeconds(0.1f);
        
        canStart = true;
        operation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        slider.maxValue = timeMinLoadScrean;
        int rand = Random.Range(0, tips.Length);
        textTips.text = "TIPS : " + tips[rand];
    }

    // Update is called once per frame
    void Update()
    {
        if (canStart)
        {
            Debug.Log("salut");
            sliderPercentText.text = ((int)(slider.normalizedValue * 100)).ToString() + " %";
            if (operation.isDone)
            {
                if (!start)
                {
                    StartCoroutine(CanLoadTODo());
                }
                slider.value += Time.unscaledDeltaTime;
                if (minTimeDone && canLoad)
                {
                    if (TransitionScript.Instance != null)
                    {
                        TransitionScript.Instance.Fade(2);
                    }
                    if (LevelManager.Instance != null)
                    {
                        
                        if (CinematicController.Instance != null && CinematicController.Instance.CanLushCinematique())
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
