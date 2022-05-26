using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip[] soundEffects, music;
    [SerializeField] [Range(0, 100)] float soundEffectsVolume, musicVolume;
    [SerializeField] AudioSource audioSourceSoundEffect, audioSourceMusic;

    // Start is called before the first frame update
    
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        instance = this;
    }
    void Start()
    {
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                PlayMusic(0);
                audioSourceMusic.pitch = 2;
                break;
            case 1:
                audioSourceMusic.pitch = 1;
                PlayMusic(1);
                break;
            case 2:
                PlayMusic(2);
                audioSourceMusic.pitch = 1;
                break;
            case 3:
                PlayMusic(3);
                break;
                audioSourceMusic.pitch = 1;
            case 4:
                PlayMusic(4);
                audioSourceMusic.pitch = 1;
                break;
            case 5:
                PlayMusic(5);
                audioSourceMusic.pitch = 1;
                break;


        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeVolumeMusic(musicVolume/100);
        ChangeVolumeSoundEFFect(soundEffectsVolume/100);
    }

    public void ChangePitch(float pitch)
    {
        audioSourceMusic.pitch = pitch;
    }

    public void ChangeVolumeSoundEFFect(float volume)
    {
        audioSourceSoundEffect.volume = volume;
    }
    public void ChangeVolumeMusic(float volume)
    {
        audioSourceMusic.volume = volume/1.5f;
    }
    public void PlayMusic(int index)
    {
        StopMusic();
        audioSourceMusic.clip = music[index];
        audioSourceMusic.Play();
    }
    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }
    public void playSoundEffect(int index)
    {
        audioSourceSoundEffect.PlayOneShot(soundEffects[index]);
    }
}
