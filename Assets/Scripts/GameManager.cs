using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiscordPresence;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    
    public GameObject Player;
    public GameObject cam;
    public GameObject GetPlayer()
    {
        return Player;
    }
    public GameObject GetCamera()
    {
        return cam;
    }
    void Awake()
    {
        gameManager = this;
    }
    private void Start()
    {

        if (PresenceManager.instance != null)
            PresenceManager.instance.ChangeInformation();
    }
}
