using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class AttackProjectile : MonoBehaviour
{
    protected Rigidbody2D rb;

    [SerializeField] protected bool destroyOnPlayer = true;
    [SerializeField] protected int damage = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTowards(Vector2 target, float speed)
    {
        rb.velocity = (target - (Vector2)transform.position).normalized * speed;
    }

    public void MoveTowards(Vector2 target, float speed, bool changeSpriteDirection)
    {
        rb.velocity = (target - (Vector2)transform.position).normalized * speed;
        if (changeSpriteDirection)
        {
            //GetComponentInChildren<Transform>().Rotate();
            //Quaternion x = Quaternion.LookRotation(target, transform.up);
            //transform.rotation = x;
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CombatManager.instance.causeDamageJuice();
            if (CombatMovement.instance.CanTakeDamage)
            {
                PlayerData.Health -= damage;
            }
            CombatMenuNavigator.instance.UpdateCombatUI();

            if (destroyOnPlayer)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

        }
    }
}
