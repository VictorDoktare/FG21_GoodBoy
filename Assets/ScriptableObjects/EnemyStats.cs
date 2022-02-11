using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Range(1, 100)]
    public int HP;
    
    [Range(1f, 70f)]
    public float speed;

    [Range(1, 50)]
    public int attackDamage;

}
