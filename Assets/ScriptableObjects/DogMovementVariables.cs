using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "movement variables", menuName = "ScriptableObject/DogMovementStats")]
public class DogMovementVariables : ScriptableObject
{
    [SerializeField][Range(1f, 100f)]
    public float movementspeed;
    
    [SerializeField, Range(1f, 360f)]
    public float rotationspeed;
}
