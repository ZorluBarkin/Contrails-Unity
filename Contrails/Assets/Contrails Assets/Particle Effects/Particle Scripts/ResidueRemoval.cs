/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidueRemoval : MonoBehaviour
{
    public float effectTime = 120f; // currently manually set but in the future can get from parent which might get it from the effect starting object
    private float effectTimer = 0f;
    // Update is called once per frame
    void Update()
    {

        if (effectTime < effectTimer)
            Destroy(gameObject);
        else
            effectTimer += Time.deltaTime;

    }
}
