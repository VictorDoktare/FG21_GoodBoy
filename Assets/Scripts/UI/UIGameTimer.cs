using TMPro;
using UnityEngine;

public class UIGameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Update()
    {
        DisplayTime();
    }
    
    private void DisplayTime()
    {
        _text.SetText("{0}:{1:00}",TimeManager.Instance.Minutes, TimeManager.Instance.Seconds);
    }
    
}
