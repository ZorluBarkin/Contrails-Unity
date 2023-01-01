using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAMissileScript : MonoBehaviour
{
    private Rigidbody rb;

    public float mass = 85.3f;

    #region Manuverability Variables
    public float maxSpeed = 2.5f * 340.29f; // in m/s 340.29f
    public float maxOveroad = 30f; // G's it can pull
    public float burnTime = 2.2f;
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

    public float distanceToTarget = 0f;

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

            //if (//out of search cone) // self destruction when lock is lost
            //{
            //    target = null;
            //    targetLocked = false;
            //    RaycastHit hit;
            //    Physics.SphereCast(transform.position, blastRadius.x, Vector3.up, out hit, blastRadius.y);
            //    Instantiate(explosionEffect, transform.position, Quaternion.identity);
            //    // do explosion
            //    Destroy(this.gameObject);
            //}

            if (targetLocked)
            {
                DoGuidance(target.transform.position);

                if ( proximityDistance > Vector3.Distance(target.transform.position, transform.position))
                {
                    //RaycastHit hit;
                    //Physics.SphereCast(transform.position, blastRadius.x, Vector3.up, out hit, blastRadius.y);
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);

                    Destroy(this.gameObject);
                }
            }
                

        }
    }

    private void DoGuidance(Vector3 impactPoint)
    {
        burnTimer += Time.deltaTime;

        Vector3 guidanceVector = impactPoint - transform.position;
        Quaternion rotation = Quaternion.LookRotation(guidanceVector);
        
        if(burnTimer > 0.2f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxOveroad * Time.deltaTime);

        if (burnTimer < burnTime)
            rb.velocity += transform.forward * 10f;
        //rb.velocity = transform.forward * Vector3.Dot(transform.forward, rb.velocity);
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

    private void SetTrackingMode() // defines the ARH to go from SA to AR
    {

    }
}
