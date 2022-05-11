using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Data_Manager : MonoBehaviour
{
    public static Data_Manager Instance;
    [SerializeField] List<MapData> _mapData;


    private void Awake()
    {
        Instance = this;
        LoadSavedGames();
    }

    public void SetRecord(float timer, int levelIndex)
    {
        if (timer < _mapData[levelIndex].GetHighScore() || _mapData[levelIndex].GetHighScore() == 0f)
        {
            _mapData[levelIndex].SetHighScore(timer);
        }
        SaveData();
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(_mapData);
        string filepath = Application.persistentDataPath + "Save.json";
        File.WriteAllText(filepath, data);
    }

    //Charge tous les record dans toutes les maps et les charges dans les Datas;
    public void LoadSavedGames()
    {

        if (Directory.Exists(Application.persistentDataPath))
        {
            string worldsFolder = Application.persistentDataPath;
            DirectoryInfo d = new DirectoryInfo(worldsFolder);

            foreach (var file in d.GetFiles("*.json"))
            {
                string data = File.ReadAllText(file.ToString());
                _mapData = JsonUtility.FromJson<List<MapData>>(data);
            }
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            return;
        }
    }
}

[System.Serializable]

public class MapData
{
    [SerializeField] string _mapName { get; }
    public string GetMapName() { return _mapName; }
    [SerializeField] float _highScore { get; set; }
    public float GetHighScore() { return _highScore; }
    public void SetHighScore(float highscore) { _highScore = highscore; }
    [SerializeField] bool _haveUnlockLevel { get; set; }
    public bool GetHaveUnlockLevel() { return _haveUnlockLevel; }
}