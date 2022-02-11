using System;
using UnityEngine;
using UnityEngine.UI;

public class UIStatHUD : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private Sprite[] _portraitSprites;
    
    [Header("Hunger Bar Settings")]
    [SerializeField] private Image _hungerFillbar;
    [SerializeField] private HungerStat _hungerStat;

    [Header("Warmth Bar Settings")]
    [SerializeField] private Image _warmthFillbar;
    [SerializeField] private Image _tempFillBar;
    [SerializeField] private Slider _wamrthThreshold;
    [SerializeField] private WarmthStat _warmthStat;
    
    [Header("Willpower Bar Settings")]
    [SerializeField] private Image _willpowerFillbar;
    [SerializeField] private WillpowerStat _willpowerStat;

    private float portraitValue;
    private void Start()
    {
        //SetIcons();
        UpdateUI();
        //SetPortrait();
    }

    public void UpdateUI()
    {
        //Update Food value
        _hungerFillbar.fillAmount =  _hungerStat.Value / 100;

        //Update Warmth value
        _warmthFillbar.fillAmount = _warmthStat.Value / 100;
        _tempFillBar.fillAmount = _warmthStat.TempValue / 100;
        _wamrthThreshold.value = _warmthStat.ThresholdValue / 100;
        
        //Update Willpower value
        _willpowerFillbar.fillAmount = _willpowerStat.Value / 100;
        
        //SetPortrait();
    }

    private void OnEnable()
    {
        //Update Food value
        _hungerFillbar.fillAmount =  _hungerStat.Value / 100;
        //_hungerFillbar.color = Color.Lerp(_hungerBarEmpty, _hungerBarFull, _hungerFillbar.fillAmount);
        
        //Update Warmth value
        _warmthFillbar.fillAmount = _warmthStat.Value / 100;
        _tempFillBar.fillAmount = _warmthStat.TempValue / 100;
        
        //Update Willpower value
        _willpowerFillbar.fillAmount = _willpowerStat.Value / 100;
        
        //SetPortrait();
    }

    // private void SetPortrait()
    // {
    //     if (_portraitSprites == null)
    //     {
    //         return;
    //     }
    //     
    //     portraitValue = _hungerStat.Value + _warmthStat.TempValue + _willpowerStat.Value;
    //
    //     if (portraitValue >= 0 && portraitValue < 100)
    //     {
    //         Debug.Log("Bad");
    //         _portrait.sprite = _portraitSprites[2];
    //     }
    //
    //     if (portraitValue >= 100 && portraitValue < 200)
    //     {
    //         Debug.Log("Good");
    //         _portrait.sprite = _portraitSprites[1];
    //     }
    //
    //     if (portraitValue >= 200 && portraitValue < 300)
    //     {
    //         Debug.Log("Awesome");
    //         _portrait.sprite = _portraitSprites[0];
    //     }
    // }
}
