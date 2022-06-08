using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

namespace ByPass
{
    public class SteamAchivement : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SteamUserStats.ClearAchievement("NEW_ACHIEVEMENT_1_0");
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void StartSucess()
        {
            if (!SteamManager.Initialized) { return; }

            SteamUserStats.SetAchievement("NEW_ACHIEVEMENT_1_0");
            SteamUserStats.StoreStats();

            Debug.Log("heyo");
        }
    }
}
