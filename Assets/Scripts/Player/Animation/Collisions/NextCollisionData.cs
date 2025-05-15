using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCollisionData : MonoBehaviour
{
    [HideInInspector] public bool isOpportunistic;
    [HideInInspector] public Vector2 knockbackForce;
    [HideInInspector] public float minDamage;
    [HideInInspector] public float maxDamage;
}
