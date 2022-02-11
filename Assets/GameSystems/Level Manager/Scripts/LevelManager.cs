using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _cutscenes;
    [SerializeField] private PostProcessProfile _postProcessVolumeProfile;

    [SerializeField] private JukeBox jukeBox;

    [Header("Warmth Clear Condition")]
    [SerializeField][Range(0,100)] private int _level2WarmthThreshold;
    [SerializeField][Range(0,100)] private int _level3WarmthThreshold;

    [Header("Day 1 ")]
    [SerializeField]
    private float _day1Temperature;
    
    [SerializeField]
    private float _day1Exposure;
    
    [Header("Day 2 ")]
    [SerializeField]
    private float _day2Temperature;
    
    [SerializeField]
    private float _day2Exposure;
    
    [Header("Day 3 ")]
    [SerializeField]
    private float _day3Temperature;
    
    [SerializeField]
    private float _day3Exposure;
    
    [Header("Level Object Changes")]
    [SerializeField] private GameObject[] _dayPrefabs;

    private GameObject _levelPrefabs;
    private Level _level = Level.DayOne;
    [SerializeField]
    private GameObject day1Post;
    [SerializeField]
    private GameObject day3Post;
    [SerializeField]
    private GameObject day2Post;

    public Level CurrentLevel => _level;
    
    public void ResetLevel()
    {
        Debug.Log("Resetting Level");
        GameManager.Instance.ResetStats();
        Reset();
    }

    public void LoadLevel()
    {
        if (_level == Level.DayThree)
        {
            _level = Level.Ending;
        }
        else if (_level != Level.Ending)
        {
            _level+=2;
        }
       
        Debug.Log($"Load Level: {_level}");
        
        switch (_level)
        {
            case Level.DayOne:
                break;
            
            case Level.DayTwo:
                GameManager.Instance.SaveCurrentStats();
                Reset();
                GameManager.Instance.SetWarmthThreshold(_level2WarmthThreshold);
                day1Post.SetActive(false);
                day2Post.SetActive(true);
                _cutscenes[0].SetActive(true);

                Destroy(_levelPrefabs);
                SpawnLevelPrefab(1);
                break;
            
            case Level.DayThree:
                GameManager.Instance.SaveCurrentStats();
                Reset();
                GameManager.Instance.SetWarmthThreshold(_level3WarmthThreshold);
                day2Post.SetActive(false);
                day3Post.SetActive(true);
                _cutscenes[1].SetActive(true);
                
                Destroy(_levelPrefabs);
                SpawnLevelPrefab(2);
                break;
            
            case Level.Ending:
                SceneManager.LoadScene(3);
                break;
        }
        
        foreach (var gameObj in GameManager.Instance.DroppedItems)
        {
            Destroy(gameObj);
        }
        
        jukeBox.SetOccasion(_level);
        jukeBox.PlaySong(_level);
    }

    private void Reset()
    {
        GameManager.Instance.ResetPlayerHealth();
        GameManager.Instance.ResetInventory();
        TimeManager.Instance.ResetClock();
        GameManager.Instance.ResetPlayerPos();
        Destroy(_levelPrefabs);
        
        foreach (var gameObj in GameManager.Instance.DroppedItems)
        {
            Destroy(gameObj);
        }

        switch (_level)
        {
            case Level.DayOne:
                SpawnLevelPrefab(0);
                break;
            case Level.DayTwo:
                SpawnLevelPrefab(1);
                break;
            case Level.DayThree:
                SpawnLevelPrefab(2);
                break;
        }
    }

    private void Awake()
    {
        _level = Level.DayOne;
        jukeBox.PlaySong(_level);
        
        
    }

    private void Start()
    {
        SpawnLevelPrefab(0);
    }

    private void OnDisable()
    {
        _postProcessVolumeProfile.GetSetting<ColorGrading>().temperature.value = -10;
    }

    private void SpawnLevelPrefab(int index)
    {
        if (_levelPrefabs != null)
        {
            Destroy(_levelPrefabs);
        }
        
        for (int i = 0; i < _dayPrefabs.Length; i++)
        {
            if (i == index)
            {
                _levelPrefabs  = Instantiate(_dayPrefabs[i],transform.position,Quaternion.identity);
            }
        }
    }
    
    public enum Level
     {
         Intro,
         DayOne,
         DayOneCutscene,
         DayTwo,
         DayTwoCutscene,
         DayThree,
         Ending,
     }
     #if UNITY_EDITOR
     private void OnGUI()
     {
         var style = new GUIStyle();
         style.normal.textColor = Color.white;
         style.fontSize = 15;
         GUI.Label(new Rect(1525,30,300,50), $"Level State: {_level}", style);
     }
     #endif
    
    //Remove for final build
    // private void Update()
    // {
    //     QASceneReset();
    //     QALevelChange();
    // }
    // private void QASceneReset()
    // {
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         ResetLevel();
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         Application.Quit();
    //     }
    // }
    // private void QALevelChange()
    // {
    //     if (Input.GetKeyDown(KeyCode.T))
    //     {
    //         LoadLevel();
    //     }
    // }
}
