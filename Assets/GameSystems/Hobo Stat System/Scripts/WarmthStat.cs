using UnityEngine;

[CreateAssetMenu(fileName = "New HoboStat", menuName = "StatSystem/HoboStat/WarmthStat")]
public class WarmthStat : BaseStat
{
    [SerializeField][Range(0,100)] private float _tempStartValue = 100;
    [SerializeField][Range(0,100)] private float _thresholdValue = 100;

    private float _tempValue;
    public float TempValue => _tempValue;
    public float ThresholdValue => _thresholdValue;

    private void Awake()
    {
        type = StatType.Warmth;
    }

    public void SetThresholdValue(float value)
    {
        _thresholdValue = value;
        if (_thresholdValue > 100)
        {
            _thresholdValue = 100;
        }
        ClampTempValue(_thresholdValue);
        _onStatChanged.Raise();
    }


    public void SetTempValue(float value)
    {

        _tempValue = value;
        if (_tempValue > 100)
        {
            _tempValue = 100;
        }
        ClampTempValue(_tempValue);
        _onStatChanged.Raise();
    }
    
    public void AddTempValue(float value)
    {

        if (_tempValue >= _baseValue)
        {
            _tempValue += value;
            ClampTempValue(_tempValue);
            _onStatChanged.Raise();
        }
        else if (_tempValue < _baseValue)
        {
            _tempValue = _baseValue + value;
            ClampTempValue(_tempValue);
            _onStatChanged.Raise();
        }
        
        if (_tempValue > 100)
        {
            _tempValue = 100;
        }
    }
        
    public void RemoveTempValue(float value)
    {
        if (_tempValue > _baseValue)
        {
            _tempValue -= value;
            if (_tempValue < 0)
            {
                _tempValue = 0;
            }
            ClampTempValue(_tempValue);
            _onStatChanged.Raise();
        }
    }
    
    protected void ClampTempValue(float value)
    {
        value = Mathf.Clamp(value, _baseValue, 100);
    }

    public override void ResetStat()
    {
        base.ResetStat();
        _tempValue = _tempStartValue;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _tempValue = _tempStartValue;
    }
}
