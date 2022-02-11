using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/VolumeData")]
public class VolumeData : ScriptableObject
{
    [Range(0,1)]
    public float musicVolume;

    [Range(0,1)]
    public float sfxVolume;

    public delegate void VolumeChanged();

    public static event VolumeChanged OnVolumeChanged;

    
    public void UpdateMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        OnVolumeChanged?.Invoke();
    }
    
    public void UpdateSFXVolume(float newVolume)
    {
        sfxVolume = newVolume;
        OnVolumeChanged?.Invoke();
    }
}
