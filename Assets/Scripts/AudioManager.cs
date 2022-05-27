using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip[] soundEffects, music;
    [SerializeField] AudioSource audioSourceSoundEffect, audioSourceMusic, audioSourceSfx3D;

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

    }

    public void ChangePitch(float pitch)
    {
        audioSourceMusic.pitch = pitch;
    }

    public void ChangeVolumeSoundEFFect(float volume)
    {
        audioSourceSoundEffect.volume = volume;
        audioSourceSfx3D.volume = volume;
    }
    public void ChangeVolumeMusic(float volume)
    {
        audioSourceMusic.volume = volume;
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
    public void playSoundEffect(int index, float volumeScale)
    {
        audioSourceSoundEffect.PlayOneShot(soundEffects[index], volumeScale);
    }
    public void playSoundEffect3D(int index, Vector3 position, float volumeScale)
    {
        audioSourceSfx3D.gameObject.transform.position = position;
        audioSourceSfx3D.PlayOneShot(soundEffects[index], volumeScale);
    }
}
