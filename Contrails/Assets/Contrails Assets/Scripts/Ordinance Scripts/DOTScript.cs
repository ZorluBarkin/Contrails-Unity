using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTScript : MonoBehaviour
{
    public float duration = 120f;
    public float damage = 110f;

    // NOTE: Napalm will damage everything but immobilize tanks
    // IDEA: make a modifier which makes armoured vehicle have %90 damage reduction

    //private void Start()
    //{
        
    //}

    private float tickTime = 0f;
    /// <summary>
    /// Colliding object has to have a rigidbody because effect does not have a RigidBody.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Ground"))
            return;

        if(tickTime < duration)
        {
            Debug.Log(tickTime);
        }
        else
            Destroy(this);

        tickTime += Time.deltaTime;
    }

    /// <summary>
    /// Used for Low performance objects that can be triggered for DOT, Example: NPC Tanks.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Ground"))
            return;

        if (tickTime < duration)
        {
            Debug.Log(tickTime);
        }
        else
            Destroy(this);

        tickTime += Time.deltaTime;
    }
}
