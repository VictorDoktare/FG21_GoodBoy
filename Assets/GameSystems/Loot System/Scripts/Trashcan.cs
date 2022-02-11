using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Trashcan : MonoBehaviour, IAttackable 
{
    [SerializeField]
    private int trashcanHealth;

    public void TakeDamage(int amount)
    {
        trashcanHealth -= 1;
        if (trashcanHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
