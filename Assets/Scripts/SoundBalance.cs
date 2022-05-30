using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBalance : MonoBehaviour
{
    [SerializeField] Transform soundPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlaySwoosh(int soundId)
    {
        
        AudioManager.instance.playSoundEffect3D(soundId, soundPosition.position, 5f);
    }
}
