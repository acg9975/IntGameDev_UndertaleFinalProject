using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 5;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        //we cannot use Moveposition in each movement check, otherwise it will only take in the one directional input
        //simply use an empty vector3, add the movement direction we want, and add use it with MovePosition later. Allows the play to go diagonally
        Vector3 MoveVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            MoveVector += Vector3.up * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveVector += Vector3.down * speed * Time.deltaTime;
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveVector += Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveVector +=  Vector3.left * speed * Time.deltaTime;
        }
        
        rb.MovePosition(transform.position + MoveVector);
    }



}
