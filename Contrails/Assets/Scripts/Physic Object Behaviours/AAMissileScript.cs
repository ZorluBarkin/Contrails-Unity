using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAMissileScript : MonoBehaviour
{
    private Rigidbody rb;

    public float mass = 85.3f;

    #region Manuverability Variables
    public float maxSpeed = 2.5f * 340.29f; // in m/s 340.29f // this might not be needed
    public float maxOveroad = 30f; // G's it can pull
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
    [HideInInspector] public bool targetLocked = false;
    public float launchRange = 0f; // in m
    public float lockRange = 0f;
    public float missileRadarRange = 0f;
    public float trackingTime = 0f; // seconds
    #endregion

    #region Explosion Variables
    public float proximityDistance = 10f; // meters
    public float explosiveFiller = 0f;
    private Vector2 blastRadius = Vector2.zero;
    [SerializeField] private GameObject explosionEffect = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (explosionEffect == null)
            explosionEffect = null; // make the default effect here

        if(target != null)
            targetLocked = true;

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.mass = mass;

        DefineTrackingStyle();
    }

    // Update is called once per frame
    void Update()
    {
        // create lift so that it can fly

        if (targetLocked)
        {
            dt = Vector3.Dot(this.transform.position.normalized, target.transform.position.normalized);
            if (Vector3.Dot(this.transform.position.normalized, target.transform.position.normalized) < 0.303) // this means it cant track
                Explode();

            if (targetLocked)
            {
                DoGuidance(target.transform.position);

                if ( proximityDistance > Vector3.Distance(target.transform.position, transform.position))
                    Explode();
                
            }
                
        }
    }

    private void DoGuidance(Vector3 impactPoint)
    {
        burnTimer += Time.deltaTime;

        Vector3 guidanceVector = impactPoint - transform.position;
        Quaternion rotation = Quaternion.LookRotation(guidanceVector);
        
        if(burnTimer > 0.2f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxOveroad * 9.81f * Time.deltaTime);

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

        //delete object
        Destroy(this.gameObject);
    }

    private void DefineTrackingStyle()
    {
        if (ARH)
        {
            SARH = false;
            IR = false;
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
}
