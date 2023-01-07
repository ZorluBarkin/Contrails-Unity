using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class AAMissileScript : MonoBehaviour
{
    private Rigidbody rb;
    public float mass = 85.3f;
    public bool Active = false;
    private readonly float warmUpTime = 2f;
    private float warmingUp = 0f;
    public GameObject propulsionEffect = null;
    [SerializeField] private float effectDeletionTimer = 3f;
    private float effectDeletionTime = 0f;
    private ParticleSystem.MainModule particleMain; // something empty
    private ParticleSystem.MinMaxGradient particleColor = new Color(0, 0, 0, 0);

    #region Maneuverability Variables
    public float maxSpeed = 2.7f * 340.29f; // in m/s 340.29f // this might not be needed //this is baseline speed
    public float maxOverload = 40f; // G's it can pull // aim9 L can pull 30g's Aim9x can pull 60 // AMRAAM pulls 30g's
    public float trackingRate = 24f; // its just better to make it 40 at this point
    public float initialSpeed = 0f;
    public float burnTime = 5.3f; // A and B models have 2.2 seconds burn time
    public float burnTimer = 0f;
    public float fuelAmount = 40f; // in kg
    public float thrust = 1206f; // in kg
    #endregion

    #region Missile Tracking Modes
    [SerializeField] private bool IR = false;
    [SerializeField] private bool allAspect = false; // only meaningful for IR missiles
    [SerializeField] private bool SARH = false;
    [SerializeField] private bool ARH = false;
    [SerializeField] private bool beamRider = false;
    #endregion

    #region Tracking Variables
    public GameObject target = null;
    private Rigidbody targetRb = null;
    [HideInInspector] public bool targetLocked = false;
    private Vector3 impactPoint = Vector3.zero;

    public float launchRange = 0f; // in m, max launch range
    public float lockRange = 0f; // range where the missile can lock onto a target
    public float missileRadarRange = 0f; // Active seacrh range
    public float trackingTime = 0f; // seconds
    public float gimbalLimit = 40f;
    public float FOV = 2.5f;

    private float distanceToTarget = -1f;
    public float leadStopDistance = -1f;
    #endregion

    #region Explosion Variables
    public float proximityDistance = 10f; // meters
    public float explosiveFiller = 0f;
    private Vector2 blastRadius = Vector2.zero;
    [SerializeField] private GameObject explosionEffect = null;
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        if (explosionEffect == null)
            explosionEffect = null; // make the default effect here

        if(target != null)
        {
            targetLocked = true;
            targetRb = target.GetComponent<Rigidbody>();
        }

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if(propulsionEffect == null)
            propulsionEffect = transform.GetChild(1).gameObject; // make trail child object always at index 1
            

        rb.mass = mass;
        rb.maxAngularVelocity = Mathf.Infinity;
        DefineTrackingStyle();

        particleMain = propulsionEffect.GetComponent<ParticleSystem>().main;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Active)
            return;

        if (warmingUp < warmUpTime)
        {
            warmingUp += Time.deltaTime;
            return;
        }

        if (targetLocked)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            CalculateLead(distanceToTarget, leadStopDistance);
            
            // self destruct conditions
            if (burnTimer > burnTime)
            {
                if (Vector3.Angle(this.transform.position, target.transform.position) > gimbalLimit / 2 || rb.velocity.magnitude < 70) // this means it cant track
                    Debug.Log("SD");//Explode();
            }
                
            if (targetLocked)
            {
                DoGuidance(impactPoint);

                if (proximityDistance > distanceToTarget)
                    Explode();
            }       
        }
        else
        {
            SearchTarget();

            if (targetLocked)
            {
                rb.velocity = transform.forward * initialSpeed;
            }
        }

        // mark the trajectory debugging
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = transform.position;
        //sphere.transform.localScale = new Vector3(10, 10, 10);
        //Destroy(sphere.GetComponent<SphereCollider>());
    }

    private GameObject SearchTarget()
    {
        GameObject currentTarget = null;

        if (IR) // FF
        {
            if (allAspect)
            {

            }
            // unless its aim9x search target in a 45 degree cone or less dependent on the model

            // send a raycast to check for high heat source, select the highest one.

            // might end up always raytyracing the target are to see flares at one point might be too costly
            targetLocked = true;
            return currentTarget;
        }
        else if (ARH) // after a while should go into active search mode first seacrh mode for ARH missiles are SARH. // FF after some point
        {
            
            targetLocked = true;
            return currentTarget;
        }
        else if (SARH) // use the aircrafts radar to get impact point (Not a beam rider, this is actually guided and only gets the target info from the launching plane)
        {

            targetLocked = true;
            return currentTarget;
        }
        // no need for target search for beam rider as it just follows the aircrafts low angle radar (one raycast forward of aircraft will be enough)

        targetLocked = false;
        return currentTarget;
    }

    private void CalculateLead(float distanceToTarget, float trackingStopDistance)
    {
        if(rb.velocity.magnitude < 250 && burnTimer < burnTime)
        {
            impactPoint = target.transform.position;
            return;
        }
            

        if (distanceToTarget > trackingStopDistance)
        {
            float interceptTime = distanceToTarget / rb.velocity.magnitude;
            impactPoint = target.transform.position + targetRb.velocity * interceptTime;
        }
        else
            impactPoint = target.transform.position;
    }

    private void DoGuidance(Vector3 impactPoint)
    {
        burnTimer += Time.deltaTime;

        Vector3 guidanceVector = impactPoint - transform.position;
        Quaternion rotation = Quaternion.LookRotation(guidanceVector);
        //Quaternion rotation = transform.Rotae(impactPoint);

        if (burnTimer > 0.2f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, trackingRate * Time.deltaTime);

        if (burnTimer <= burnTime)
            rb.AddForce(transform.forward * (thrust * Physics.gravity.magnitude / fuelAmount * 0.60f - Physics.gravity.magnitude), ForceMode.Acceleration); // 
                                                                                                                                                            //rb.AddForce(transform.forward * 1206 * Physics.gravity.magnitude, ForceMode.Force);
                                                                                                                                                            //rb.velocity += transform.forward * 5f; // 5f for now
        else
        {
            DestroyTrailParticle();
            // this is not finished do this sometime
            //rb.velocity += new Vector3(maxOverload * Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.right) / 90,
            //    0f, 
            //    maxOverload * Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.forward) / 90);
        }
            
            //rb.velocity = transform.forward * rb.velocity.magnitude; // works but turn too fast at a point
    }

    private void Explode()
    {
        if (propulsionEffect != null)
            DestroyTrailParticle();

        // do explosion
        //RaycastHit hit;
        //Physics.SphereCast(transform.position, blastRadius.x, Vector3.up, out hit, blastRadius.y);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Debug.Log("Range Gone: " + Vector3.Distance(new Vector3(0, 2000, 0), transform.position));
        Debug.Log("Distance to Target: " + distanceToTarget);
        //delete object
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Used to destroy the particle
    /// </summary>
    private void DestroyTrailParticle()
    {
        if (propulsionEffect != null)
        {
            if (effectDeletionTime == 0)
            {
                propulsionEffect.transform.parent = null;
                particleColor = particleMain.startColor;
            }

            particleColor.color = new Color(1, 1, 1, Mathf.Clamp01((effectDeletionTimer - effectDeletionTime) / effectDeletionTimer));
            particleMain.startColor = particleColor;

            propulsionEffect.transform.position += propulsionEffect.transform.forward * 1f;

            if (effectDeletionTimer * 2f < effectDeletionTime)
                Destroy(propulsionEffect);

            effectDeletionTime += Time.deltaTime;
        }
        else
        {
            propulsionEffect = null;
        }
    }

    private void DefineTrackingStyle()
    {
        if (ARH)
        {
            IR = false;
            SARH = false;
            beamRider= false;
        }
        else if (SARH)
        {
            IR = false; 
            ARH = false;
            beamRider= false;
        }
        else if (beamRider)
        {
            IR= false;
            ARH = false;
            SARH= false;
        }
        else // default is IR
        {
            SARH= false;
            ARH = false;
            beamRider= false;
        }
    }

    /// <summary>
    /// This decides Weather to go Active or Semi-Active radar per missile.
    /// </summary>
    private void SetTrackingMode() // defines the ARH to go from SA to AR
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.forward + transform.position, impactPoint);
    }

}
