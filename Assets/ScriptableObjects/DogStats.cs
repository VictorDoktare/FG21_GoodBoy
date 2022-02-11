using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Dog Stats")]
public class DogStats : ScriptableObject
{
    [Range(1, 200)]
    public int maxHealth;

    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Range(1, 10)]
    public int multiAttackHits;

    [Range(0.01f, 2f)]
    public float attackWindow;

    private float currentAttackWindow;

    [Range(1, 10)]
    public int attackDamage;

    private float currentAttackDamage;

    [Range(1f, 50f)]
    public float movementSpeed;

    private float currentMovementSpeed;
    
    [Range(1f, 360f)]
    public float turnSpeed;

    private float currentTurnSpeed;

    [SerializeField]
    private GameEvent onPlayerDeath;
    

    public void InitStats()
    {
        currentHealth = maxHealth;
        currentAttackWindow = attackWindow;
        currentAttackDamage = attackDamage;
        currentMovementSpeed = movementSpeed;
        currentTurnSpeed = turnSpeed;
    }

    private void OnEnable()
    {
        InitStats();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Debug.Log("player died");
            onPlayerDeath.Raise();
            currentHealth = maxHealth;
        }
    }
}
