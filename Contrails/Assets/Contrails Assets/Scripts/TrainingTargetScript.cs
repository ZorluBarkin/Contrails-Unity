/*  
 * Copyright 2023 Barkın Zorlu 
 * All rights reserved.
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/4.0/ or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
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
