using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IAttackable
{
    [SerializeField]
    private EnemyStats stats;

    [SerializeField]
    private Renderer enemyRenderer;

    [SerializeField]
    private Color damageFlashColor = Color.red;

    [SerializeField]
    private float damageFlashDuration = 0.2f;

    [SerializeField]
    private GameObject deathPoofprefab;
    
    private Material enemyMaterial;

    private void Awake()
    {
        enemyMaterial = enemyRenderer.material;
    }

    private int health;
    

    private IEnumerator DamageFlash(float duration)
    {
        enemyMaterial.SetColor("_Emission", damageFlashColor);
        yield return new WaitForSeconds(duration);
        enemyMaterial.SetColor("_Emission", Color.black);
    }

    private void Start()
    {
        health = stats.HP;
    }

    public void TakeDamage(int amount)
    {
        StartCoroutine(DamageFlash(damageFlashDuration));
        health -= amount;
        if (health <= 0)
        {
            Instantiate(deathPoofprefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    
}
