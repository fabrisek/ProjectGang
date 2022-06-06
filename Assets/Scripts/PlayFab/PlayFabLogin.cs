using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Steamworks;

public class PlayFabLogin : MonoBehaviour
{
    public static PlayFabLogin Instance;
    [SerializeField] string entityId;
    [SerializeField] string entityType;
    [SerializeField] string playfabId;

    public string GetEntityId() { return entityId; }

    public string GetPlayFabId() { return playfabId; }
    public string GetEntityType() { return entityType; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        Instance = this;
        DontDestroyOnLoad(this.gameObject);            
    }

    private void Start()
    {
        Login();
        print(SteamUser.GetSteamID().ToString());
    }

    public void Login()
    {
        if (!SteamManager.Initialized) { return; }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SteamUser.GetSteamID().ToString(),
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnError);
    }

    void OnLogin(LoginResult result)
    {
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;
        playfabId =  result.PlayFabId;
        OnUpdatePlayerName();
    }

    public void OnUpdatePlayerName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = SteamFriends.GetPersonaName()
        }, result =>
        {
            Debug.Log("The player's display name is now: " + result.DisplayName);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

}