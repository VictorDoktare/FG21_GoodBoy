using UnityEngine;

public class BaseStat : ScriptableObject
{
        [SerializeField] protected GameEvent _onStatChanged;
        public Sprite Icon;
        public StatType type;

        [Header("Value Setting")]
        [SerializeField][Range(0,100)] private float _startValue = 100;

        protected float _baseValue;
        public float Value => _baseValue;

        public virtual void SetValue(float value)
        {
                _baseValue = value;
                if (_baseValue > 100)
                {
                        _baseValue = 100;
                }
                ClampValue(_baseValue);
                _onStatChanged.Raise();
        }

        public virtual void AddValue(float value)
        {
                _baseValue += value;
                if (_baseValue > 100)
                {
                        _baseValue = 100;
                }
                ClampValue(_baseValue);
                _onStatChanged.Raise();
        }
        
        public virtual void RemoveValue(float value)
        {
                _baseValue -= value;
                if (_baseValue < 0)
                {
                        _baseValue = 0;
                }
                ClampValue(_baseValue);
                _onStatChanged.Raise();
        }
        
        protected void ClampValue(float value)
        {
                value = Mathf.Clamp(value, 0f, 100);
        }

        public virtual void ResetStat()
        {
                _baseValue = _startValue;
                _onStatChanged.Raise();
        }
        
        public virtual void OnEnable() => _baseValue = _startValue;
}

public enum StatType
{
        Hunger,
        Warmth,
        Willpower
}