using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float castDistance;
    [SerializeField] private LayerMask interactionLayer;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, OverworldMovement.Direction, castDistance, interactionLayer);

            if (hit.collider != null)
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                interactable.TriggerInteraction();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, OverworldMovement.Direction * castDistance);
    }
}
