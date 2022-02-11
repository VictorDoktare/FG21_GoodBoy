using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DropType
{
    Food,
    Warmth,
    Willpower,
}

public class ResourceDropPoint : MonoBehaviour
{
    [SerializeField]
    private DropType dropType;

    public DropType DropType => dropType;

    private void Start()
    {
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawWireSphere(transform.position, 1f);
        
    }

#endif
}
