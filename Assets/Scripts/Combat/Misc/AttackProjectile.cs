using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class AttackProjectile : MonoBehaviour
{
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTowards(Vector2 target, float speed)
    {
        rb.velocity = (target - (Vector2)transform.position).normalized * speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Damage player");
            // CombatManager.DamagePlayer();
            Destroy(gameObject);
        }
    }
}
