using UnityEngine;

[CreateAssetMenu(fileName = "New HoboStat", menuName = "StatSystem/HoboStat/HungerStat")]
public class HungerStat : BaseStat
{
    [Header("De-buffed by stat")]
    [SerializeField] private WillpowerStat stat;

    [Header("Events")]
    [SerializeField] private GameEvent onHungerFail;

    public void DecreaseHunger()
    {
        // Johan:
        // Formula was -= Mathf.Abs(-0.25f * (stat.Value - 90f))
        // So if willpower = 100, it becomes -0.25f * (100f - 90f), which is -2.5f.
        _baseValue -= Mathf.Abs((-0.25f * (stat.Value - 100f)) + -3f);
        ClampValue(_baseValue);
        _onStatChanged.Raise();
    }

    public void CheckHungerValue()
    {
        if (_baseValue <= 0)
        {
            onHungerFail.Raise();
        }
    }
    
    private void Awake()
    {
        type = StatType.Hunger;
    }
}