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

public class PodCapScript : MonoBehaviour
{

    private float deleteTime = 10f;
    private float deleteTimer = 0f;

    public GameObject target = null; // test

    void FixedUpdate()
    {
        if(deleteTimer >= deleteTime || transform.position.y < 1f)
            Destroy(gameObject);

        deleteTimer += Time.deltaTime;

    }
}
