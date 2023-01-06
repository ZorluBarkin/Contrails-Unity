using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPodScript : MonoBehaviour
{
    public GameObject rocket = null;
    public Rigidbody aircraftsRigidBody = null;

    public bool armed = false;
    private bool empty = false;
    
    public bool launch = false;
    public float deviation = 0f;
    public int rocketCount = 0;

    #region Launch Variables
    public float launchInterval = 0.2f;
    private float launchIntervalTimer = 0f;
    public float maxLaunchSpeed = 200; // m/s 
    #endregion

    private Vector3 LaunchPosition = Vector3.zero;

    private void Start()
    {
        if(aircraftsRigidBody == null)
            aircraftsRigidBody = transform.parent.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (empty)
        {
            // enable jettison

            launchIntervalTimer = 0f;
            return;
        }

        if (launch && armed && maxLaunchSpeed < aircraftsRigidBody.velocity.magnitude) // test the last statement
        {
            // pop the caps (Model to a capcule)


            if (launchInterval < launchIntervalTimer && rocketCount > 0)
            {
                LaunchPosition = transform.forward * 3 + transform.position;

                Instantiate(rocket, LaunchPosition,  transform.rotation);
                rocketCount--;
                launchIntervalTimer = 0f;
            }
            else if(rocketCount ==  0)
                empty = true;

            launchIntervalTimer += Time.deltaTime;
        }

    }
}
