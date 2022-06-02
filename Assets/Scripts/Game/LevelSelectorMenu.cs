using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectorMenu : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] int _indexData;
    [SerializeField] GameObject textGoPress;
    int _indexScene;
    [SerializeField]int _indexWorld;

    [SerializeField] TextMeshProUGUI _nameLevel;
    [SerializeField] TextMeshProUGUI _star;
    public int GetSceneNumber() { return _indexScene; }
    public int GetIndexData() { return _indexData; }
    // Start is called before the first frame update
    void Start()
    {
        _indexScene = Data_Manager.Instance.GetMapData(_indexData, _indexWorld).GetIndexScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ChangeInformation()
    {
        DATA data = Data_Manager.Instance.GetData();
        _nameLevel.text = data._worldData[_indexWorld].WorldName;
        int totalStar = 0;
        int starUnlock = 0;
        for (int i = 0; i < data._worldData[_indexWorld]._mapData.Count; i++)
        {
            MapData mapData = data._worldData[_indexWorld]._mapData[i];
            for (int j = 0; j < mapData.TimeStar.Length; j++)
            {
                totalStar++;
                if (mapData.GetHighScore() <= mapData.TimeStar[j] && mapData.GetHighScore() != 0)
                {
                    starUnlock++;
                }
            }
            
        }
        _star.text = "STAR : " + starUnlock.ToString() + " / " + totalStar.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            _canvas.SetActive(true);
            other.GetComponent<MenuAntCrontroller>().SetLevelRef(this);
            ChangeInformation();
            textGoPress.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            textGoPress.SetActive(false);
            _canvas.SetActive(false);
            other.GetComponent<MenuAntCrontroller>().SetLevelRef(null);
        }
    }
}
