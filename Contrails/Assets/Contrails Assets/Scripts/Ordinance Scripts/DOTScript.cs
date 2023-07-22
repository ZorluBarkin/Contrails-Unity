/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTScript : MonoBehaviour
{
    [HideInInspector] public float duration = 120f;
    [HideInInspector] public float damage = 110f;

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

    float damageDone = 0;
    /// <summary>
    /// Used for Low performance objects that can be triggered for DOT, Example: NPC Tanks.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay(Collider collision)
    {
        // TODO: when enemy dies check by layer, if dead dont activate or when enbemy dies remove collider

        if (collision.gameObject.layer == LayerMask.GetMask("Ground"))
            return;

        if (tickTime < duration)
        {
            damageDone += damage * Time.deltaTime / 5;
            //Debug.Log(damageDone);
        }
        else
            Destroy(this);

        tickTime += Time.deltaTime;
    }
}
