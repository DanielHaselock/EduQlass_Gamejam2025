using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NextCollisionData))]
public class KickCollision : MonoBehaviour
{
    [SerializeField] private BoxCollider2D footCollider;
    private NextCollisionData collisionData;
    void Start()
    {
        collisionData = GetComponent<NextCollisionData>();
        footCollider.enabled = false;
    }

    public void activateFootCollider() //activated in animator
    {
        footCollider.enabled = true;
    }
    public void deactivateFootCollider()
    {
        footCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            if (!collision.gameObject.GetComponent<Health>() || footCollider.enabled == false) //fixes multiple colliders "hitting"
                return;

            float diff = transform.position.x - collision.transform.position.x;

            if (diff < 0)
                diff = 1;
            else
                diff = -1;

            Vector2 finalForce = new Vector2(collisionData.knockbackForce.x * diff, collisionData.knockbackForce.y);

            Power power = GetComponent<Power>();

            float damage = Mathf.Lerp(collisionData.minDamage, collisionData.maxDamage, power.currPower / power.maxPower);

            collision.gameObject.GetComponent<Health>().receiveHit(damage, finalForce, collisionData.isOpportunistic); //damage is set in the animation clip

            deactivateFootCollider();
        }
    }
}
