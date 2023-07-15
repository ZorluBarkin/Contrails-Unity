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

public class RocketPodScript : MonoBehaviour
{
    public GameObject rocket = null;
    private Rigidbody rb = null;
    public Rigidbody aircraftsRigidBody = null;
    public Mesh capOffShape = null;
    public GameObject capShape = null;
    public int capNumber = 2;
    public float launcherLenght = 1.0f; // 1f for the ffar might mouse, 2f for zuni
    public float launcherDiameter = 0.4f; // 0.5 for zuni, 0.4 for 7x might mouse and 0.6 for 39x mighty mouse
    public float initialSpeed = 0.0f;

    public bool armed = false; // they also have a safety distance which is 47m for Might mouse. (no need for simulating)
    public bool launch = false;
    private bool empty = false;
    private bool jettisoned = false;
    public bool jettison = false;

    #region Launch Variables
    public float launchInterval = 0.2f;
    private float launchIntervalTimer = 0f;
    public float maxLaunchSpeed = 200; // m/s
    public float deviation = 2f; // inaccuracy in degrees
    public int rocketCount = 7; // 7 is small pod and 19 is large pod for mighty mouse, zuni only comes with 2 or 4 can be stacked 3 times
    private bool popCap = true;
    #endregion

    private Vector3 LaunchPosition = Vector3.zero;

    private void Start()
    {
        if(aircraftsRigidBody == null)
            aircraftsRigidBody = transform.parent.parent.GetComponent<Rigidbody>();

        transform.position -= transform.up * (launcherDiameter / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (jettison)
        {
            jettison = false;
            empty = true;

            this.gameObject.transform.parent = null;
            
            if(rb == null)
            {
                rb = this.gameObject.AddComponent<Rigidbody>();
                rb.mass = 100f;
                rb.drag = 0.295f;
                rb.velocity = transform.forward * aircraftsRigidBody.velocity.magnitude;
            }

            jettisoned = true;
        }

        if (empty)
        {
            if(transform.position.y < 5f && jettisoned)
                Destroy(this.gameObject);

            launch = false;
            armed = false;
            launchIntervalTimer = 0f;
            return;
        }

        if (launch && armed && maxLaunchSpeed > aircraftsRigidBody.velocity.magnitude) // test the last statement
        {
            // pop the caps (Model to a capcule)
            if (popCap)
            {
                transform.GetChild(0).GetComponent<MeshFilter>().mesh = capOffShape;
                transform.GetChild(0).transform.localScale = new Vector3(launcherDiameter, launcherLenght / 2, launcherDiameter); // divide lenght by 2 because cylinders are 1:2 ratiod not 1:1

                // spawn caps which pop off, 2 max 1-0 min
                Vector3 spawnPos = transform.position;
                spawnPos += transform.forward * launcherLenght;

                for(int i = 0; i < capNumber; i++)
                {
                    if(i > 0)
                    {
                        spawnPos -= transform.forward * (launcherLenght * 2);
                        GameObject podCap = Instantiate(capShape, spawnPos, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                        podCap.GetComponent<Rigidbody>().velocity = this.transform.forward * aircraftsRigidBody.velocity.magnitude;
                    }
                    else
                    {
                        GameObject podCap = Instantiate(capShape, spawnPos, transform.rotation);
                        podCap.GetComponent<Rigidbody>().velocity = this.transform.forward * aircraftsRigidBody.velocity.magnitude;
                    }                             
                }

                popCap = false;
            }

            // launching the rockets
            if (launchInterval < launchIntervalTimer && rocketCount > 0)
            {
                LaunchPosition = transform.forward * 3 + transform.position;

                Instantiate(rocket, LaunchPosition, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation), 
                    Random.Range(-deviation, deviation), Random.Range(-deviation, deviation))); 
                // script has an aircraftsRigidBody.velocity.magnitude might add to rockets as well
                rocketCount--;
                launchIntervalTimer = 0f;
            }
            else if(rocketCount ==  0)
                empty = true;

            launchIntervalTimer += Time.deltaTime;
        }

    }

}
