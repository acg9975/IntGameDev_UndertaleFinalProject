using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{
    public static OverworldMovement instance;

    //public static Vector2 PlayerPosition { get { return instance.transform.position; } }

    [SerializeField] private float speed = 5;

    private Vector3 move;
    private Vector3 dir;

    private Rigidbody2D rb;
    private Animator anim;

    public static bool canMove = true;
    public item item;

    public static Vector3 Direction { get { return (instance == null) ? Vector3.up : instance.dir; } }

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

            if (move != Vector3.zero)
                dir = move;

            anim.SetInteger("Horizontal", (int) move.x);
            anim.SetInteger("Vertical", (int) move.y);
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
