using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public AudioClip ambient;
    public VolumeData volumeData;
    private AudioSource ambientSource;
    
    // Start is called before the first frame update
    void Awake()
    {
        ambientSource = GetComponent<AudioSource>();
    }

    public void UpdateAudio()
    {
        ambientSource.volume = volumeData.sfxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
