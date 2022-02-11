using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }

    [SerializeField] private DogStats _dogStats;
    [SerializeField] private GameData gameData;
    [SerializeField] private GameStateEvent gameStateEvent;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camera;

    private List<GameObject> _droppedItems = new List<GameObject>();
    public List<GameObject> DroppedItems { get => _droppedItems; set => _droppedItems = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        SaveCurrentStats();
    }
    
    public void ToggleMouse()
    {
        if (Cursor.visible == false)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        if (Cursor.lockState == CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void EndLevel()
    {
        var baseValue = gameData.warmth.Value;
        var tempValue = gameData.warmth.TempValue;
        var threshold = gameData.warmth.ThresholdValue;
        
        if (baseValue >= threshold || tempValue >= threshold)
        {
            LevelCleared();
        }
        else
        {
            LevelFailed();
        }
    }
    public void ResetStats()
    {
        gameData.ResetStatData();
    }

    public void ResetInventory()
    {
        gameData.ResetInventoryData();
    }

    public void ResetPlayerHealth()
    {
        _dogStats.InitStats();
    }

    public void ResetPlayerPos()
    {
        _player.SetActive(false);
        _camera.GetComponent<SimpleOrbitalCamera>().enabled = true;
        _player.GetComponent<PlayerResetPosition>().ResetPlayerPos();
        _player.SetActive(true);
        
    }

    public void SetWarmthThreshold(int value)
    {
        gameData.warmth.SetThresholdValue(value);
    }

    public void LevelFailed()
    {
        gameStateEvent.onLevelNotCleared.Raise();
        //_player.SetActive(false);
        _camera.GetComponent<SimpleOrbitalCamera>().enabled = false;
    }

    public void LevelCleared()
    {
        gameStateEvent.onLevelCleared.Raise();
    }

    public void SaveCurrentStats()
    {
        gameData.SaveStats();
    }
    
#if UNITY_EDITOR
    private void OnGUI()
    {
        var style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 15;
        GUI.Box(new Rect(1500,5,300,180), "Debug");
        GUI.Label(new Rect(1525,70,300,50), $"Warmth: {gameData.warmth.Value}", style);
        GUI.Label(new Rect(1525,90,300,50), $"Warmth Temp: {gameData.warmth.TempValue}", style);
        GUI.Label(new Rect(1525,110,300,50), $"Hunger: {gameData.hunger.Value}", style);
        GUI.Label(new Rect(1525,130,300,50), $"Willpower: {gameData.willpower.Value}", style);
    }
#endif
}
