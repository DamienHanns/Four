using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public event System.Action OnDeath;

    public float maxhealth = 3.0f;

    [SerializeField] protected float currentHealth;
    protected bool bIsDead;

    protected virtual void Start()
    {
        currentHealth = maxhealth;
    }

    public virtual void TakeDamage(float damage, RaycastHit hit)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && ! bIsDead) 
        {
            Die();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !bIsDead)
        {
            Die();
        }
    }

    void Die()
    {
        bIsDead = true;
        if (OnDeath != null) { OnDeath(); }
        Destroy(gameObject);
    }

}
