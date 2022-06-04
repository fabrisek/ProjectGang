using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
[System.Serializable]
public class DATA
{
    public List<WorldInfo> _worldData;
    public StatsPlayer statPlayer;

}
public class Data_Manager : MonoBehaviour
{
    public static Data_Manager Instance;
    [SerializeField] DATA Data;

    public DATA GetData() { return Data; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
        LoadSavedGames();
    }

    public void SetRecord(float timer, int levelIndex, int worldIndex)
    {
        if (Data._worldData[worldIndex]._mapData[levelIndex].GetHighScore() == 0)
        {
            Data._worldData[worldIndex]._mapData[levelIndex].SetHighScore(timer);
            print(timer);
            if (PlayFabHighScore.Instance)
                PlayFabHighScore.Instance.SendLeaderBord(timer, Data_Manager.Instance.GetData()._worldData[worldIndex]._mapData[levelIndex].GetMapName());
        }

        if (timer < Data._worldData[worldIndex]._mapData[levelIndex].GetHighScore())
        {
            Data._worldData[worldIndex]._mapData[levelIndex].SetHighScore(timer);
            if (PlayFabHighScore.Instance)
                PlayFabHighScore.Instance.SendLeaderBord(timer, Data_Manager.Instance.GetData()._worldData[worldIndex]._mapData[levelIndex].GetMapName());
        }

        if (levelIndex + 1 != Data._worldData[worldIndex]._mapData.Count)
            Data._worldData[worldIndex]._mapData[levelIndex + 1].SetHaveUnlockLevel(true);

        SaveData();
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(Data);
        string filepath = Application.persistentDataPath + "/Save.json";
        File.WriteAllText(filepath, data);

        print(data);
    }

    //Charge tous les record dans toutes les maps et les charges dans les Datas;
    public void LoadSavedGames()
    {

        string worldsFolder = Application.persistentDataPath + "/Save.json";
        if (File.Exists(worldsFolder))
        {
            string fileContents = File.ReadAllText(worldsFolder);
            DATA data = JsonUtility.FromJson<DATA>(fileContents);

            if (data._worldData.Count == Data._worldData.Count)
            {
                for (int i = 0; i < data._worldData.Count; i++)
                {
                    if (data._worldData[i]._mapData.Count != Data._worldData.Count)
                    {
                        return;
                    }
                }
                return;
            }
            Data = data;
        }

    }

    public MapData GetMapData(int index, int worldIndex) { return Data._worldData[worldIndex]._mapData[index]; }
}

[System.Serializable]

public class MapData
{
    [SerializeField] string _mapName;
    [SerializeField] int _indexScene;
    public int GetIndexScene() { return _indexScene; }
    public string GetMapName() { return _mapName; }
    [SerializeField] float _highScore;
    public float GetHighScore() { return _highScore; }
    public void SetHighScore(float highscore) { _highScore = highscore; }
    [SerializeField] bool _haveUnlockLevel;
    public bool GetHaveUnlockLevel() { return _haveUnlockLevel; }
    public void SetHaveUnlockLevel(bool unlock) { _haveUnlockLevel = unlock; }

    public float[] TimeStar;

    public Sprite spriteLevel;
}

[System.Serializable]
public class WorldInfo
{
    public string WorldName;
    public bool HaveUnlockWorld;
    public List<MapData> _mapData;
}

[System.Serializable]
public class StatsPlayer
{

}