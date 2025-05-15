using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class On_ground : MonoBehaviour
{
    public bool onGround = true;

    private LayerMask groundLayerMask;


    //Events with no arguments
    private List<UnityAction> collisionEnterEvents = new List<UnityAction>(); 
    private List<UnityAction> collisionExitEvents = new List<UnityAction>();

    private void Start()
    {
        this.groundLayerMask = LayerMask.NameToLayer("Ground");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayerMask.value)
        {
            this.onGround = true;
            collisionEnterEvents.ForEach(ev => { ev.Invoke(); });
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == groundLayerMask.value)
        {
            this.onGround = false;
            collisionExitEvents.ForEach(ev => { ev.Invoke(); });
        }
    }

    public void addCollisionEnterEvent(UnityAction ua)
    {
        if (!collisionEnterEvents.Contains(ua))
        {
            collisionEnterEvents.Add(ua);
        }
    }

    public void removeCollisionEnterEvent(UnityAction ua)
    {
        collisionEnterEvents.Remove(ua); //automatically checks for element
    }

    public void addCollisionExitEvent(UnityAction ua)
    {
        if (!collisionExitEvents.Contains(ua))
        {
            collisionExitEvents.Add(ua);
        }
    }

    public void removeCollisionExitEvent(UnityAction ua)
    {
        collisionExitEvents.Remove(ua); //automatically checks for element
    }
}
