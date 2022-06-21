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
    public int LastWorld;
}

public class Data_Manager : MonoBehaviour
{
    public static Data_Manager Instance;
    [SerializeField] DATA Data;
    public static bool AlreadyInGame { get; set; }
    public static ControlDeviceType LastDevice;
    public DATA GetData() { return Data; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        }
        else
        {

            DontDestroyOnLoad(this.gameObject);
            Instance = this;
            LoadSavedGames();
        }
    }

    public void SetRecord(float timer, int levelIndex, int worldIndex)
    {
        if (Data._worldData[worldIndex]._mapData[levelIndex].GetHighScore() == 0)
        {
            Data._worldData[worldIndex]._mapData[levelIndex].SetHighScore(timer);
            print(timer);
            if (PlayFabHighScore.Instance)
                PlayFabHighScore.Instance.SendLeaderBord(timer, Data_Manager.Instance.GetData()._worldData[worldIndex]._mapData[levelIndex].GetSceneData().MapName);

            if (PhantomeControler.Instance != null)
            {
                Data._worldData[worldIndex]._mapData[levelIndex].SetPhantomeSave(PhantomeControler.Instance.phantomeSave);
            }
        }

        if (timer < Data._worldData[worldIndex]._mapData[levelIndex].GetHighScore())
        {
            Data._worldData[worldIndex]._mapData[levelIndex].SetHighScore(timer);
            if (PlayFabHighScore.Instance)
                PlayFabHighScore.Instance.SendLeaderBord(timer, Data_Manager.Instance.GetData()._worldData[worldIndex]._mapData[levelIndex].GetSceneData().MapName);

            if (PhantomeControler.Instance != null)
            {
                Data._worldData[worldIndex]._mapData[levelIndex].SetPhantomeSave(PhantomeControler.Instance.phantomeSave);
            }
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
    }

    //Charge tous les record dans toutes les maps et les charges dans les Datas;
    public void LoadSavedGames()
    {

        string worldsFolder = Application.persistentDataPath + "/Save.json";
        if (File.Exists(worldsFolder))
        {
            string fileContents = File.ReadAllText(worldsFolder);
            DATA data = JsonUtility.FromJson<DATA>(fileContents);
            Data.LastWorld = data.LastWorld;

            for (int i = 0; i < Data._worldData.Count; i++)
            {
                if (data._worldData[i] != null)
                {
                    Data._worldData[i].HaveUnlockWorld = data._worldData[i].HaveUnlockWorld;

                    for (int j = 0; j < Data._worldData[i]._mapData.Count; j++)
                    {
                        if (data._worldData[i]._mapData[j] != null)
                        {
                            Data._worldData[i]._mapData[j].SetHighScore(data._worldData[i]._mapData[j].GetHighScore());
                            Data._worldData[i]._mapData[j].SetHaveUnlockLevel(data._worldData[i]._mapData[j].GetHaveUnlockLevel());
                            Data._worldData[i]._mapData[j].SetPhantomeSave(data._worldData[i]._mapData[j].GetPhantomSave());
                            //Data._worldData[i]._mapData[j] DATA IN GAME
                            // data._worldData[i]._mapData[j] DATA VIA JSON "SAVE";
                        }
                    }
                }
            }
        }
    }

    public MapData GetMapData(int index, int worldIndex) { return Data._worldData[worldIndex]._mapData[index]; }

}

[System.Serializable]

public class MapData
{
    [SerializeField] SceneObject _sceneData;
    public SceneObject GetSceneData() { return _sceneData; }
    [SerializeField] float _highScore;
    public float GetHighScore() { return _highScore; }
    public void SetHighScore(float highscore) { _highScore = highscore; }
    [SerializeField] bool _haveUnlockLevel;
    public bool GetHaveUnlockLevel() { return _haveUnlockLevel; }
    public void SetHaveUnlockLevel(bool unlock) { _haveUnlockLevel = unlock; }
    [SerializeField] PhantomeSave _savePhantom;
    public void SetPhantomeSave(PhantomeSave save) { _savePhantom = save; }
    public PhantomeSave GetPhantomSave() { return _savePhantom; }
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