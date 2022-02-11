using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum SongOccasion
{
    Intro,
    DayOne,
    DayOneCutscene,
    DayTwo,
    DayTwoCutscene,
    DayThree,
    DayThreeCutscene,
    Ending,
}

[Serializable]
struct SongSet
{
    [SerializeField]
    public LevelManager.Level SongOccasion;
    [SerializeField]
    public AudioClip[] songs;
}
public class JukeBox : MonoBehaviour
{
    [FormerlySerializedAs("cutsceneSongs")]
    [SerializeField]
    private SongSet[] songSets;

    [SerializeField, Range(0,1)]
    private float volume;

    [SerializeField]
    private VolumeData volumeData;

    public bool playOnAwake;

    public LevelManager.Level songOnAwake;

    private AudioSource musicBox;

    private LevelManager.Level currentOccasion = LevelManager.Level.Intro;

    private Dictionary<LevelManager.Level, SongSet> songLibrary = new Dictionary<LevelManager.Level, SongSet>();

    private void Awake()
    {
        musicBox = GetComponent<AudioSource>();
        musicBox.volume = volume;
        musicBox.playOnAwake = playOnAwake;

        currentOccasion = songOnAwake;

        LoadSongs();
    }

    private void OnEnable()
     {
         VolumeData.OnVolumeChanged += UpdateVolume;
     }

    private void UpdateVolume()
     {
         musicBox.volume = volumeData.musicVolume;
     }

    public void PlayDayOneMusic()
    {
        var occasionClips = songLibrary[LevelManager.Level.DayOne].songs;
        
        if (musicBox.clip == occasionClips[0] && occasionClips.Length > 1)
        {
            musicBox.clip = occasionClips[1];
        }
        else
        {
            musicBox.clip = occasionClips[0];;
        }
    }
    
    public void PlayDayOneCutsceneMusic()
    {
        var occasionClips = songLibrary[LevelManager.Level.DayOneCutscene].songs;
        
        if (musicBox.clip == occasionClips[0] && occasionClips.Length > 1)
        {
            musicBox.clip = occasionClips[1];
        }
        else
        {
            musicBox.clip = occasionClips[0];;
        }
    }
    
    public void PlayDayTwoMusic()
    {
        var occasionClips = songLibrary[LevelManager.Level.DayTwo].songs;
        
        if (musicBox.clip == occasionClips[0] && occasionClips.Length > 1)
        {
            musicBox.clip = occasionClips[1];
        }
        else
        {
            musicBox.clip = occasionClips[0];;
        }
    }
    
    public void PlayDayTwoCutsceneMusic()
    {
        var occasionClips = songLibrary[LevelManager.Level.DayTwoCutscene].songs;
        
        if (musicBox.clip == occasionClips[0] && occasionClips.Length > 1)
        {
            musicBox.clip = occasionClips[1];
        }
        else
        {
            musicBox.clip = occasionClips[0];;
        }
    }
    
    public void PlayDayThreeMusic()
    {
        var occasionClips = songLibrary[LevelManager.Level.DayThree].songs;
        
        if (musicBox.clip == occasionClips[0] && occasionClips.Length > 1)
        {
            musicBox.clip = occasionClips[1];
        }
        else
        {
            musicBox.clip = occasionClips[0];;
        }
    }


    private void StopMusic(Scene arg0, LoadSceneMode arg1)
     {
         StopMusic();
     }


     private void LoadSongs()
    {
        foreach (var songSet in songSets)
        {
            songLibrary[songSet.SongOccasion] = songSet;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playOnAwake)
            PlaySong(songOnAwake);
    }

    public void SetOccasion(LevelManager.Level newOccasion)
    {
        currentOccasion = newOccasion;
    }

    public void PlaySong(LevelManager.Level occasion)
    {
        musicBox.Stop();
        var occasionClips = songLibrary[occasion].songs;
        
        if (musicBox.clip == occasionClips[0] && occasionClips.Length > 1)
        {
            musicBox.clip = songLibrary[occasion].songs[1];
        }
        else
        {
            musicBox.clip = songLibrary[occasion].songs[0];
        }
        
        musicBox.Play();
        StartCoroutine(QueueNextSong());
    }

    private IEnumerator QueueNextSong()
    {
        yield return new WaitForSeconds(musicBox.clip.length);
        PlaySong(currentOccasion);
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        musicBox.Stop();
    }
    
    
}
