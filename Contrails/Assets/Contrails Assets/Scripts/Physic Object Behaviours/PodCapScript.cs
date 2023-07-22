/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
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
