using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackCooldownSeconds = 0.5f;
    public GameObject enemy;

    private float attackCooldownTimer;

    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && attackCooldownTimer <= 0f && enemy == GameObject.FindWithTag("Enemy"))
        {
            Attack();

            attackCooldownTimer = attackCooldownSeconds;
        }
    }

    void Attack()
    {
        Collider2D[] attackedObjects = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D attackedObject in attackedObjects)
        {
            var damageable = attackedObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

public interface IDamageable
{
    void TakeDamage(int amount);
}
