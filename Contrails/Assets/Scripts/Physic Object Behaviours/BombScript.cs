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

    #region shape Bools
    public bool sphere = false;
    public bool shortCylinder = false;
    public bool midCylidner = false;
    public bool capcule = false;
    public bool longCylinder = false;
    #endregion

    public float dropVelocity = 0f; // the velocity when the bomb was dropped from the plane

    #region Guidance Variables
    public bool guided = false;
    public bool targetSet = false;
    [HideInInspector] public Vector3 impactPoint = Vector3.zero;
    public GameObject target = null;
    public float turnRate = 5f;
    #endregion

    #region Explosion Variables
    public float TNTPayload = 0f;
    public Vector2 blastRadius = Vector2.zero;
    private BoxCollider boxCollider = null;
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

        if(boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        if(rb == null)
            rb = GetComponent<Rigidbody>();

        rb.drag = SetDragCoefficient();
        rb.mass = mass;
        rb.velocity = Vector3.forward * dropVelocity;
    }

    private void Update()
    {
        if (targetSet)
        {
            if(target != null)
                impactPoint = target.transform.position;

            if (guided)
                DoGuidance(impactPoint);
        }

        if (timerStarter)
        {
            fuzeTimer += Time.deltaTime;

            if(fuzeTimer >= fuzeTime)
                Explode();
            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        timerStarter = true;
    }

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
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        //Debug.Log(hit.transform);

        //delete object
        Destroy(this.gameObject);
    }

    private float SetDragCoefficient()
    {
        if (sphere)
        {
            shortCylinder = false;
            midCylidner = false;
            capcule = false;
            longCylinder = false;
            return 0.47f;
        } 
        else if (shortCylinder)
        {
            sphere = false;
            midCylidner = false;
            capcule = false;
            longCylinder = false;
            return 0.6f;
        }  
        else if (midCylidner)
        {
            sphere = false;
            shortCylinder = false;
            capcule = false;
            longCylinder = false;
            return 0.35f;
        }
        else if (longCylinder)
        {
            sphere = false;
            shortCylinder = false;
            midCylidner = false;
            capcule = false;
            return 0.24f;
        }
        else if (capcule)
        {
            sphere = false;
            shortCylinder = false;
            midCylidner = false;
            longCylinder = false;
            return 0.295f;
        }
        else // default is the sphere body
        {
            sphere = true;
            shortCylinder = false;
            midCylidner = false;
            capcule = false;
            longCylinder = false;
            return 0.47f;
        }
        
    }

}
