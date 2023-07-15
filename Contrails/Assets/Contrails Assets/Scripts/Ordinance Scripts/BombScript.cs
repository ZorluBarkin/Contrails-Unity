/*  
 * Copyright 2023 Barkın Zorlu 
 * All rights reserved.
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/4.0/ or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
 */

using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private Rigidbody rb = null;

    public float mass = 227f; // in kg
    public float bombWidth = 0.5f;

    public float dropVelocity = 0f; // the velocity when the bomb was dropped from the plane, Vertical dispersion + 5f ensures a drop forwards
    public bool armed = false;
    public bool launch = false;
    private bool launched = false;
    public float deviation = 10f; // in degrees // 10 8 6 5
    public float verticalDispersion = 5f; // in m/s
    public float maxLaunchSpeed = 340.29f * 1.5f; // 1.5 mach
    private float calculatedDrag = 1.52f / 3;
    public float terminalVelocity = -1; // if -1 then dont use this calculation

    #region Guidance Variables
    // if the bomb is dumb then there will be no target set whcih will block guidance
    public enum GuidanceType
    {
        MCLOS, // Radio
        infrared, // Self homing, temperature
        LaserGuided, // semi active
        ElectroOptical, // self homing tv guided
        Satellite // laser tracking but rays are from god
    }
    public GuidanceType guidanceType = GuidanceType.LaserGuided;

    public bool targetSet = false;
    [HideInInspector] public Vector3 impactPoint = Vector3.zero;
    public GameObject target = null;
    public float turnRate = 5f;
    #endregion

    #region Explosion Variables
    public float payload = 0f; // in kg, Tritonal explosive
    
    private float TNTPayload = 0f;
    public Vector2 blastRadius = Vector2.zero;
    [SerializeField] private GameObject explosionEffect = null; // assign in editor

    // fuze
    private bool timerStarter = false;
    public float fuzeTime = 0f;
    private float fuzeTimer = 0f;
    #endregion


    public float testTime = 0f; // test
    private float factor = 2f; // test
    // Start is called before the first frame update
    void Start()
    {
        if (explosionEffect == null)
            explosionEffect = null; // make the default effect here

        if(rb == null)
            rb = GetComponent<Rigidbody>();

        transform.position -= transform.up * (bombWidth / 2);

        TNTPayload = 2.25f * payload; // 9 MJ/kg (Tritonal) = 4 MJ/Kg (TNT) at the same weight, Hence, tritonal is 2.25 times the weight of TNT.
        //Time.timeScale = 30f; // for testing

        if (terminalVelocity > 0) // prefered calculation type
            calculatedDrag = Physics.gravity.magnitude / terminalVelocity; // max acceleration Divided by terminal velocity gives ideal drag 
        else // should be used if there is no terminal velocity data available and a drag coefficient data exists
            rb.drag /= 3;
        //factor = 8.9683f * Mathf.Pow(transform.position.y, -1.29f); // wrong
        //Debug.Log(factor);
        float time = Mathf.Sqrt(rb.mass / (Physics.gravity.magnitude * factor * rb.drag) * (float)System.Math.Acosh(System.Math.Pow(System.Math.E, (transform.position.y * factor * rb.drag) / rb.mass)));
        Debug.Log(time);
    }

    private void Update()
    {
        testTime += Time.deltaTime; // test
        if (!armed)
            return;

        if (launch)
        {
            dropVelocity = FlightScript._speed;

            if (dropVelocity <= verticalDispersion)
                dropVelocity = verticalDispersion + 5f;

            if ( dropVelocity > maxLaunchSpeed )
            {
                Debug.Log("Too fast to drop"); // turn this to an on screen warning
                launch = false;
                return;
            }

            launch = false;
            launched = true;

            transform.parent = null;
            //transform.position -= transform.up * 2f;
            //transform.rotation = transform.rotation * Quaternion.Euler(
            //    Random.Range(-deviation, deviation),
            //    Random.Range(-deviation, deviation),
            //    Random.Range(-deviation, deviation));

            rb.drag = calculatedDrag;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = transform.forward * (dropVelocity + Random.Range(-verticalDispersion, verticalDispersion));
        }

        if (targetSet && launched)
        {
            impactPoint = TrackTarget();
            //if (target != null) 
            //    impactPoint = target.transform.position;

            DoGuidance(impactPoint);
        }
        else
        {
            if(rb != null)
                transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
            

        if (timerStarter)
        {
            fuzeTimer += Time.deltaTime;
            Debug.Log(testTime); // test
            if(fuzeTimer >= fuzeTime)
                Explode();
            
        }

    }

    // to make it go forward, this emulates the real life distance a bomb can travel
    private void FixedUpdate()
    {
        rb.AddForce(rb.mass * Physics.gravity.magnitude * new Vector3(transform.forward.x, 0, transform.forward.z));
    }

    /// <summary>
    /// Traking done according to tracking type of bomb.
    /// </summary>
    private Vector3 TrackTarget()
    {
        switch (guidanceType)
        {
            case GuidanceType.infrared:
                return Vector3.zero; // fill in
            case GuidanceType.LaserGuided:
                // send a laser from plane respecting the planes view angels update in a regular interval
                return Vector3.zero; // fill in
            case GuidanceType.ElectroOptical:
                // look through bombs camer with BW shader lock and give target.transform.position
                return Vector3.zero; // fill in
            case GuidanceType.Satellite:
                // do laser guiding but laser comes from god
                return Vector3.zero; // fill in
            default: // default is mclos
                // change where the impact point is based on pilot input
                return Vector3.zero; // fill in
        }
    }

    /// <summary>
    /// Guides the bomb on target
    /// </summary>
    /// <param name="impactPoint"></param>
    private void DoGuidance(Vector3 impactPoint)
    {
        Vector3 guidanceVector = impactPoint - transform.position;
        Quaternion rotation = Quaternion.LookRotation(guidanceVector);
        rotation = Quaternion.Euler(Mathf.Clamp(rotation.eulerAngles.x, 45, 135), rotation.eulerAngles.y, rotation.eulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnRate * Time.deltaTime);
        rb.velocity = transform.forward * Vector3.Dot(transform.forward, rb.velocity);
    }

    private void Explode()
    {
        // do explosion
        RaycastHit hit;
        Physics.SphereCast(transform.position, blastRadius.x, Vector3.up, out hit, blastRadius.y);

        if (explosionEffect != null && Vector3.Angle(Camera.main.transform.forward, transform.position - Camera.main.transform.position) < 100f)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        //Debug.Log(hit.transform);

        //delete object
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
            return;

        timerStarter = true;
    }

}
