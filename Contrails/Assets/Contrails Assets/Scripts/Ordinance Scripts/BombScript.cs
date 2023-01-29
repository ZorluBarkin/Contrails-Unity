using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private Rigidbody rb = null;

    public float mass = 227f; // in kg
    public float bombWidth = 0.5f;

    public float dropVelocity = 0f; // the velocity when the bomb was dropped from the plane, 40 ensures to drop forwards
    public bool armed = false;
    public bool launch = false;
    private bool launched = false;
    public float deviation = 10f; // in degrees // 10 8 6 5
    public float verticalDispersion = 5f; // in m/s
    private float maxLaunchSpeed = 340.29f * 1.5f; // 1.5 mach

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
    public float dragCoefficient = 0.155f; // for mk82 its max at 0.155
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
    }

    private void Update()
    {
        if (!armed)
            return;

        if (launch)
        {
            dropVelocity = FlightScript._speed;

            if( dropVelocity > maxLaunchSpeed )
            {
                Debug.Log("Too fast to drop"); // turn this to on screen warning
                launch = false;
                return;
            }

            launch = false;
            launched = true;

            transform.parent = null;
            transform.position -= transform.up * 2f;
            transform.rotation = transform.rotation * Quaternion.Euler(
                Random.Range(-deviation, deviation), 
                Random.Range(-deviation, deviation), 
                Random.Range(-deviation, deviation));

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

            if(fuzeTimer >= fuzeTime)
                Explode();
            
        }

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
