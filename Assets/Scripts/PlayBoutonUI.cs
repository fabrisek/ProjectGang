using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    public class PlayBoutonUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void PlaySoundOnClick ()
        {
            AudioManager.instance.playSoundEffect(0, 1);
        }

        public void PlaySoundOnIt()
        {
            AudioManager.instance.playSoundEffect(0, 1);
        }

        public void PlaySoundRemoveIt()
        {
            AudioManager.instance.playSoundEffect(0, 1);
        }
    }
}
