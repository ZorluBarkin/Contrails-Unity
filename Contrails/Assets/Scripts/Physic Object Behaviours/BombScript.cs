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
    public Vector3 impactPoint = Vector3.zero;
    #endregion

    #region Explosion Variable
    public float TNTPayload = 0f;
    private BoxCollider boxCollider = null;

    // fuze
    public bool timerStarter = false;
    public float fuzeTime = 0f;
    private float fuzeTimer = 0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
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
        if (guided && targetSet)
        {   
            DoGuidance(impactPoint);
        }

        if (timerStarter)
        {
            fuzeTimer += Time.deltaTime;

            if(fuzeTimer >= fuzeTime)
            {
                // do explosion
                //Physics.SphereCast();

                //delete object
                fuzeTimer = 0f;
                Destroy(this.gameObject);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        timerStarter = true;
    }

    private void DoGuidance(Vector3 impactPoint)
    {
        //return;
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
            return 0.25f;
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
