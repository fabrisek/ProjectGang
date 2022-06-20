using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip[] soundEffects, music;
    [SerializeField] AudioSource audioSourceSoundEffect, audioSourceMusic, audioSourceSfx3D;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float generalVolume;
    string actualSnapShot;
    public void SetGeneralVolume(float volume)
    {
        generalVolume = volume;
        UpdateVolume();
    }

    // Start is called before the first frame update
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
       
    }
    void Start()
    {
        SetGeneralVolume(Settings.VolumeGeneral);
        ChangeVolumeSoundEFFect(Settings.VolumeSFX);
        ChangeVolumeMusic(Settings.VolumeMusic);

        switch (SceneManager.GetActiveScene().buildIndex)
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
                audioSourceMusic.pitch = 1;
                break;
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

    }

    public void ChangePitch(float pitch)
    {
        audioSourceMusic.pitch = pitch;
    }

    public void UpdateVolume()
    {
        audioSourceSoundEffect.volume *= generalVolume;
        audioSourceSfx3D.volume *= generalVolume;
        audioSourceMusic.volume *= generalVolume;
    }
    public void ChangeVolumeSoundEFFect(float volume)
    {
        audioSourceSoundEffect.volume  = volume * generalVolume;
        audioSourceSfx3D.volume = volume * generalVolume;

    }
    public void ChangeVolumeMusic(float volume)
    {
        audioSourceMusic.volume = volume * generalVolume;

    }
    public void PlayMusic(int index)
    {
        StopMusic();
        if(index < music.Length)
        {
            audioSourceMusic.clip = music[index];
        }
        else
        {
            audioSourceMusic.clip = music[2];
        }
        
        audioSourceMusic.Play();
    }
    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }
    public void playSoundEffect(int index, float volumeScale)
    {
        audioSourceSoundEffect.PlayOneShot(soundEffects[index], volumeScale);
    }
    public void playSoundEffect3D(int index, Vector3 position, float volumeScale)
    {
        audioSourceSfx3D.gameObject.transform.position = position;
        audioSourceSfx3D.PlayOneShot(soundEffects[index], volumeScale);
    }

    public void ChangeMixerSnapShot (string name, float timeToReach)
    {
        audioMixer.FindSnapshot(name).TransitionTo(timeToReach);
    }

}
