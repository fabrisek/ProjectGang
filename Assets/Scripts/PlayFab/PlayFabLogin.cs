using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

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
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnError);
    }

    void OnLogin(LoginResult result)
    {
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;
        playfabId =  result.PlayFabId;
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

}