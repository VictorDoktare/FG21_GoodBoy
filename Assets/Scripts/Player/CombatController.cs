using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider attackHitbox;

    [SerializeField]
    private DogStats dogStats;

    [SerializeField]
    private float attackHitboxSize = 1f;
    
    public LayerMask attackableLayer;

    private RaycastHit[] attackHits;

    private float debugAttacklen;

    // Start is called before the first frame update
    void Awake()
    {
        attackHitbox.enabled = false;

        attackHits = new RaycastHit[10];
    }

    private void OnEnable()
    {
        DogControllerRedux.OnAttackPressed += EnableAttackBox;
    }

    private void OnDisable()
    {
        DogControllerRedux.OnAttackPressed -= EnableAttackBox;
    }

    public void EnableAttackBox(float length)
    {
        attackHitbox.enabled = true;
        // StartCoroutine(AttackWindow(duration));

        int hits = Physics.SphereCastNonAlloc(transform.position, attackHitboxSize, transform.forward, attackHits, length,  attackableLayer);

        debugAttacklen = length;

        int attacks = 0;

        for (int i = 0; i < hits; i++)
        {
            if (attackHits[i].transform.TryGetComponent(out IAttackable entity) && attacks < dogStats.multiAttackHits)
            {
                entity.TakeDamage(dogStats.attackDamage);
                attacks++;
            }
        }
    }

    IEnumerator AttackWindow(float duration)
    {
        yield return new WaitForSeconds(duration);
        attackHitbox.enabled = false;
    }

    private void OnDrawGizmos()
    {
        var prevcol = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackHitboxSize);
        Gizmos.color = prevcol;
        if (attackHitbox.enabled)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, transform.forward * debugAttacklen);
        }
    }
}
