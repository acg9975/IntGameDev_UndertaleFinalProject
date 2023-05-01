using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSystem : MonoBehaviour
{

    [SerializeField]
    private item keyItem;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (PlayerData.inventory.getItem(keyItem))
            {
                PlayerData.inventory.useItem(keyItem);
                Destroy(gameObject);
            }
        }
    }



}
