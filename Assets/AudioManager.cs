using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] soundEffects, music;
    [SerializeField] [Range(0, 100)] float soundEffectsVolume, musicVolume;
    [SerializeField] AudioSource audioSourceSoundEffect, audioSourceMusic;

    // Start is called before the first frame update
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        PlayMusic(0);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeVolumeMusic(musicVolume/100);
        ChangeVolumeSoundEFFect(soundEffectsVolume/100);
    }

    public void ChangeVolumeSoundEFFect(float volume)
    {
        audioSourceSoundEffect.volume = volume;
    }
    public void ChangeVolumeMusic(float volume)
    {
        audioSourceMusic.volume = volume;
    }
    public void PlayMusic(int index)
    {
        Debug.Log("playMusic");
        audioSourceMusic.clip = music[index];
        audioSourceMusic.Play();
    }
    public void playSoundEffect(int index)
    {
        audioSourceSoundEffect.PlayOneShot(soundEffects[index]);
    }
    
    
}
