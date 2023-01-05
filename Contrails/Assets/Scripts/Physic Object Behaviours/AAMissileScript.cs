using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAMissileScript : MonoBehaviour
{
    private Rigidbody rb;
    public float mass = 85.3f;
    public bool Active = false;
    private readonly float warmUpTime = 2f;
    private float warmingUp = 0f;

    #region Maneuverability Variables
    public float maxSpeed = 2.7f * 340.29f; // in m/s 340.29f // this might not be needed //this is baseline speed
    public float maxOveroad = 30f; // G's it can pull // aim9 L can pull 30g's Aim9x can pull 60 // AMRAAM pulls 30g's
    public float initialSpeed = 0f;
    public float burnTime = 6f; // A and B models have 2.2 seconds burn time
    public float burnTimer = 0f;
    #endregion

    #region Missile Tracking Modes
    [SerializeField] private bool IR = false;
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

        rb.mass = mass;
        rb.maxAngularVelocity = Mathf.Infinity;
        DefineTrackingStyle();
        
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
                if (Vector3.Angle(this.transform.position, target.transform.position) > 45 || rb.velocity.magnitude < 50) // this means it cant track
                    Explode();
            }
                
            if (targetLocked)
            {
                DoGuidance(impactPoint);

                if ( proximityDistance > distanceToTarget)
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

    }

    private GameObject SearchTarget()
    {
        GameObject currentTarget = null;

        if (IR) // FF
        {
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

        if (burnTimer > 0.2f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxOveroad * 9.81f);

        if (burnTimer < burnTime)
            rb.velocity += transform.forward * 5f; // 5f for now
        //rb.velocity = transform.forward * Vector3.Dot(transform.forward, rb.velocity);
    }

    private void Explode()
    {
        // do explosion
        //RaycastHit hit;
        //Physics.SphereCast(transform.position, blastRadius.x, Vector3.up, out hit, blastRadius.y);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Debug.Log("Range Gone: " + Vector3.Distance(new Vector3(0, 2000, 0), transform.position));
        Debug.Log("Distance to Target: " + distanceToTarget);
        //delete object
        Destroy(this.gameObject);
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
