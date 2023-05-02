using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    public static CombatMovement instance;

    public static Vector2 PlayerPosition { get { return instance.transform.position; } }

    [SerializeField] private float speed = 5;

    private Vector3 move;

    private Rigidbody2D rb;

    public static bool canMove = true;

    private void Awake()
    {
        instance = this;

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

    public void damageJuice()
    {
        //flash the sprite renderer
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();//GetComponent<SpriteRenderer>();

        StartCoroutine(damageJuiceRoutine(sr));

    }
    private IEnumerator damageJuiceRoutine(SpriteRenderer sr)
    {

        float timetoStopFlashing = Time.time + 2f;
        //Debug.Log(timetoStopFlashing + " vs " + Time.time);
        while (Time.time <= timetoStopFlashing && PlayerData.IsAlive)
        {
            sr.enabled = false;
            //Debug.Log("inactive");
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            //Debug.Log("active");
            yield return new WaitForSeconds(0.1f);
        }
        //Debug.Log("active");
        sr.enabled = true;



    }
}
