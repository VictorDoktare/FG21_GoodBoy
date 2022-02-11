using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName= "ScriptableObject/Dialogue", fileName = "Dialogue")]
[System.Serializable]
public class Dialogue : ScriptableObject
{
    public string name;
    
    //Adds a text area for the dialogue that is between 3-10 lines long.
    [TextArea(3, 10)]
    public List<string> sentences;
}
