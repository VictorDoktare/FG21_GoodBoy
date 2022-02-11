using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableEntity : MonoBehaviour, ITargetable
{
    [Header("Targetable")]
    [SerializeField]
    private bool targetable = true;
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    public GameObject targetingCircle;

    public bool Targetable => targetable;
    public Transform TargetTransform => targetTransform;

    public void ToggleCircle(bool active)
    {
        targetingCircle.SetActive(active);
    }
}
