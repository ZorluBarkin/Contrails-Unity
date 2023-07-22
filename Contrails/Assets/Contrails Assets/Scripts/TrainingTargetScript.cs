/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTargetScript : MonoBehaviour
{
    public Rigidbody rb = null;
    public float velocity = 0f;

    // Start is called before the first frame update
    private void Start() // currently private might make it public later
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * velocity;
    }

    //private void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawSphere(transform.position + transform.forward * rb.velocity.magnitude, 5f);
    //}

}
