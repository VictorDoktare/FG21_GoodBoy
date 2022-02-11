using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TextBoxLocation
{
    Top,
    Bottom,
    Left,
    Right
}

[CreateAssetMenu(menuName = "Cutscene/Cutscene Slide")]
public class CutsceneSlide : ScriptableObject
{
    [SerializeField]
    public Sprite spriteGraphic;
    
    [SerializeField, TextArea] 
    public string text;

    [SerializeField]
    public TextBoxLocation textLocation;
    
    [SerializeField]
    public float pauseTime;
}
