using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBalance : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip swooshSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySwoosh()
    {
        audioSource.PlayOneShot(swooshSound);
    }
}
