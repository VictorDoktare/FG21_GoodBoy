using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance { get => instance; set => instance = value; }

    [Header("Events")]
    [SerializeField] private GameEvent onEndTimer;
    [SerializeField] private GameEvent onTickEvent;

    [Header("Settings")] 
    [SerializeField] [Range(0,59)] private int minutes;
    [SerializeField] [Range(0,59)] private int seconds;
    [SerializeField] [Range(0,60)] private int ticksPerMinute;

    private int minuteStartValue;
    private int secondStartValue;

    public int Minutes => minutes;
    public int Seconds => seconds;

    private float _time;
    private float _tickCount;

    #region UnityEventFunctions

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
    }

    private void Start()
    {
        secondStartValue = seconds;
        minuteStartValue = minutes;
    }

    private void OnValidate()
    {
        SetValues();
    }

    private void OnEnable()
    {
        SetValues();
    }
    
    private void Update()
    {
        CheckForTick();

        if (_time > 1)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            _time = 0;
            onEndTimer.Raise();
        }

        minutes = MinuteCounter(_time);
        seconds = SecondCounter(_time);
    }

    #endregion

    private void CheckForTick()
    {
        if (_tickCount > 0)
        {
            _tickCount -= Time.deltaTime;
        }
        else
        {
            Debug.Log("TimeTick");
            onTickEvent.Raise();
            _tickCount += 60 / ticksPerMinute;
        }
    }

    private int MinuteCounter(float value)
    {
        if (value < 0)
        {
            value = 0;
        }

        return Mathf.FloorToInt(value / 60);
    }

    private int SecondCounter(float value)
    {
        if (value < 0)
        {
            value = 0;
        }

        return Mathf.FloorToInt(value % 60);
    }

    private void SetValues()
    {
        _time = (minutes * 60) + (seconds + 1);
        _tickCount = 60 / ticksPerMinute;
    }

    public void ResetClock()
    {
        seconds = secondStartValue;
        minutes = minuteStartValue;
        SetValues();
    }
}