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
    public float initialSpeed = 0.0f;

    public bool armed = false;
    private bool empty = false;
    private bool jettisoned = false;
    public bool jettison = false;

    #region Launch Variables
    public bool launch = false;
    public float launchInterval = 0.2f;
    private float launchIntervalTimer = 0f;
    public float maxLaunchSpeed = 200; // m/s
    public float deviation = 2f; // inaccuracy in degrees
    public int rocketCount = 0;
    private bool popCap = true;
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

            launchIntervalTimer = 0f;
            return;
        }

        if (launch && armed && maxLaunchSpeed > aircraftsRigidBody.velocity.magnitude) // test the last statement
        {
            // pop the caps (Model to a capcule)
            if (popCap)
            {
                transform.GetChild(0).GetComponent<MeshFilter>().mesh = capOffShape;
                transform.GetChild(0).transform.localScale = new Vector3(0.5f, 1f, 0.5f);

                // spawn caps which pop off, 2 max 1-0 min
                Vector3 spawnPos = transform.position;
                spawnPos += transform.forward * 2f;

                for(int i = 0; i < capNumber; i++)
                {
                    if(i > 0)
                    {
                        spawnPos -= transform.forward * 4f;
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

                Instantiate(rocket, LaunchPosition,  transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation), 
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
