using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] float range;
    [SerializeField] LayerMask npc;
    [SerializeField] LayerMask gate;


    private void OnInteract(InputValue Value)
    {
        NpcInteract();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if((1<<collider.gameObject.layer & gate) != 0)
        {
            collider.GetComponent<IInteractable>().Interact(this);
        }
    }

    Collider2D[] colliders = new Collider2D[20];
    private void NpcInteract()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders, npc);
        for (int i = 0; i < size; i++)
        {
            IInteractable interactable = colliders[i].GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
                break;
            }
        }
    }







    private void OnDrawGizmosSelected()
    {
        if (debug == false)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
