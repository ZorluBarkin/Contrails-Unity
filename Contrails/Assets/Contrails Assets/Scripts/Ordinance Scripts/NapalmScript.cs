using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NapalmScript : MonoBehaviour
{

    public Rigidbody rb = null;
    public GameObject explosionEffect = null; // the size is 22.5x90m^2, spawn one and keep spawning until about 90m then reduce y scale to 5, keep time to delete
    public GameObject model = null;

    public float width = 0.475f;
    public float dropVelocity = 0f;
    public float weight = 340f;

    public bool armed = false;
    public bool launch = false;
    private bool launched = false;
    public float deviation = 10f; // in degrees // 10 8 6 5
    public float verticalDispersion = 5f; // in m/s
    private float rotateSpeed = 5f;

    //public float rollingDistance = 90f;
    public float burnDuration = 120f;
    public Vector2 burnTemp = new Vector2(900, 1300);

    // Start is called before the first frame update
    void Start()
    {
       if(rb == null)
            rb = GetComponent<Rigidbody>();
        if (model == null)
            model = transform.GetChild(0).gameObject;

        transform.position -= transform.up * (width / 2);
    }

    void FixedUpdate()
    {
        if (launched)
        {
            model.transform.Rotate(0, 0, rotateSpeed); // 5 is good
            return;
        }

        if (!armed)
            return;

        if (launch)
        {
            dropVelocity = FlightScript._speed;
            //if(dropVelocity > maxLaunchSpeed)
            //{
            //    Debug.Log("Too fast to drop");
            //    launch = false;
            //    return;
            //}

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
            rb.mass = weight;
            rb.velocity = transform.forward * (dropVelocity + Random.Range(-verticalDispersion, verticalDispersion));
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
            
    }

    private void Explode()
    {
        
        Quaternion rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        GameObject go =  Instantiate(explosionEffect, transform.position, rotation);
        NapalmEffectScript napalmEffect = go.GetComponent<NapalmEffectScript>();//.burnTemperature = burnTemp;

        napalmEffect.burnTime = burnDuration;
        napalmEffect.burnTemperature = burnTemp;
        //napalmEffect.rollingDistance = rollingDistance;

        Destroy(gameObject); // Destroy(gameObject, 3); // maybe for 3 seconds move forward spawning particle along the way
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
            return;

        Explode();
    }
}
