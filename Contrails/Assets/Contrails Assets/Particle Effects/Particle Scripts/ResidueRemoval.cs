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
