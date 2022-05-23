using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class PlayFabHighScore : MonoBehaviour
{
    public static PlayFabHighScore Instance;
    GameObject prefabScoreTitle;
    Transform scoreboardParent;

    public void InitializeHighScore(GameObject prefabScoreTiles, Transform parent)
    {
        prefabScoreTitle = prefabScoreTiles;
        scoreboardParent = parent;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void SendLeaderBord(float score, string nameMap)
    {
        score *= 1000;
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = nameMap,
                    Value = (int)score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("LeaderBorad Send");
    }

    public void GetLeaderBord(string mapName)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = mapName,
            StartPosition = 0,
            MaxResultsCount = 50
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    public void GetLeaderBoardAroundPlayer(string mapName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = mapName,
            MaxResultsCount = 50
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }

    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        foreach (Transform item in scoreboardParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(prefabScoreTitle, scoreboardParent);
            TextMeshProUGUI[] text = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            text[0].text = "#" + item.Position.ToString();
            text[1].text = "User : " + item.DisplayName;
            text[2].text = "Score : " + Timer.FormatTime((float)item.StatValue / 1000);

            
            if (item.PlayFabId == PlayFabLogin.Instance.GetEntityId())
            {
                text[0].color = Color.red;
                text[1].color = Color.red;
                text[2].color = Color.red;
            }
        }
    }


    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in scoreboardParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(prefabScoreTitle, scoreboardParent);
            TextMeshProUGUI[] text = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            text[0].text = "#" + item.Position.ToString();
            text[1].text = "User : " + item.DisplayName;
            text[2].text = "Score : " + Timer.FormatTime((float)item.StatValue / 1000);
            //newGo.GetComponentInChildren<HighScoreButton>().SetPlayerTitleId(item.PlayFabId);
            if (item.PlayFabId == PlayFabLogin.Instance.GetPlayFabId())
            {
                text[0].color = Color.red;
                text[1].color = Color.red;
                text[2].color = Color.red;
            }    
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
}
