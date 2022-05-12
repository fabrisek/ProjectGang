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
            CustomId = ((int)Random.Range(0, 999999)).ToString(),
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