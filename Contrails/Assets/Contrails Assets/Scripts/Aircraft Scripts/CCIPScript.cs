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

public class CCIPScript : MonoBehaviour
{
    // Constantly computed impact point.
    public WeaponType selectedWeapon = WeaponType.Empty;

    public Rigidbody rb = null;
    public GameObject ordinance = null;

    public bool CCRP = false;
    public bool CCIP = false;

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {
        // Compute impact point
        // idea: drop velocity (m/s) divided by Altitude (m) gives seconds of drop
        // 
    }

}
