using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IAttackable
{
    public int hitsToBreak = 3;

    public void TakeDamage(int amount)
    {
        hitsToBreak -= 999999;
        if (hitsToBreak <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
