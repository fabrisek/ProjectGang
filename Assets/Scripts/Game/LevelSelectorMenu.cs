using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectorMenu : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] int _indexData;
    int _indexScene;

    [SerializeField] TextMeshProUGUI _nameLevel;
    [SerializeField] TextMeshProUGUI _highScore;
    public int GetSceneNumber() { return _indexScene; }
    public int GetIndexData() { return _indexData; }
    // Start is called before the first frame update
    void Start()
    {
        _indexScene = Data_Manager.Instance.GetMapData(_indexData).GetIndexScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeInformation()
    {
        MapData data = Data_Manager.Instance.GetMapData(_indexData);
        _nameLevel.text = data.GetMapName();
        _highScore.text = Timer.FormatTime(data.GetHighScore());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            MapData data = Data_Manager.Instance.GetMapData(_indexData);
            if (data.GetHaveUnlockLevel())
            {
                //_canvas.SetActive(true);
                //ChangeInformation();
                other.GetComponent<MenuAntCrontroller>().SetLevelRef(this);
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            //_canvas.SetActive(false);
            other.GetComponent<MenuAntCrontroller>().SetLevelRef(null);
        }
    }
}
