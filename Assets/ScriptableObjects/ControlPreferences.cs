using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "User Preferences/Controls Preferences")]
public class ControlPreferences : ScriptableObject
{
    public float mouseSensitivity;
    
    [Range(-1, 1), Tooltip("Should be either 1, or -1")]
    public int invertedVertical;
    
    [Range(-1, 1), Tooltip("Should be either 1, or -1")]
    public int invertedHorizontal;
}
