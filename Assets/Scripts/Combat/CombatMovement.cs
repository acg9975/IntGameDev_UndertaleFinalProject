using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    private Vector3 move;

    private Rigidbody2D rb;

    public static bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // the direction the player should move is updated each frame
        if (canMove)
        {
            move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


            if (move.x > 0)
                move.x = 1;
            if (move.x < 0)
                move.x = -1;

            if (move.y > 0)
                move.y = 1;
            if (move.y < 0)
                move.y = -1;
        }

    }

    private void FixedUpdate()
    {
        // the player moves on FixedUpdate() for consistent movement each frame
        if (canMove)
        {
            rb.MovePosition(transform.position + move * speed * Time.fixedDeltaTime);

        }
    }
}
