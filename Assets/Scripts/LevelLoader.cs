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
        AudioManager.instance.StopMusic();
        
        StartCoroutine(LoadAsynchronously(sceneIndex));
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
            yield return null;
            _loadingScreenPanel.SetActive(false);
        }
    }
}
