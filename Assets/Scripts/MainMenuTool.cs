using UnityEngine;
using System.Collections;
using DiscordPresence;
public class MainMenuTool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (PresenceManager.instance != null)
            PresenceManager.instance.ChangeInformation();

    }
}

