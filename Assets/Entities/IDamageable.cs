using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, RaycastHit hit);

    void TakeDamage(float damage);

}
