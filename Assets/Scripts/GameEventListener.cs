using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour, IGameEventListener
{
    [Header("Game Event to Listen to")]
    [SerializeField] private GameEvent _event;
    [Header("Response to Game Event")]
    [SerializeField] private UnityEvent _response;

    private void OnEnable()
    {
        if (_event != null)
        {
            _event.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        _event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        _response?.Invoke();
    }
}
