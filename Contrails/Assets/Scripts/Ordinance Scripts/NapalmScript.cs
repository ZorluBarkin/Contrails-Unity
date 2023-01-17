using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NapalmScript : MonoBehaviour
{

    public Rigidbody rb = null;
    public GameObject explosionEffect = null;
    public float dropVelocity = 0f;
    public float weight = 340f;

    public bool armed = false;
    public bool launch = false;
    private bool launched = false;
    public float deviation = 10f; // in degrees // 10 8 6 5
    public float verticalDispersion = 5f; // in m/s
    private float rotateSpeed = 5f;

    public float burnTime = 10; // in mins
    public Vector2 explosionRadius = Vector2.zero; // its very long also wide but very lenghty

    // Start is called before the first frame update
    void Start()
    {
       if(rb == null)
            rb = GetComponent<Rigidbody>();

        burnTime *= 60;
    }

    void FixedUpdate()
    {
        if (launched)
        {
            transform.Rotate(rotateSpeed, 0, 0); // 5 is good
            return;
        }
        
        if (launch)
        {
            launch = false;
            launched = true;

            transform.parent = null;
            transform.position -= transform.up * 2f;
            transform.rotation = transform.rotation * Quaternion.Euler(
                Random.Range(-deviation, deviation), 
                Random.Range(-deviation, deviation), 
                Random.Range(-deviation, deviation));

            rb.useGravity = true;
            rb.mass = weight;
            rb.velocity = transform.forward * (dropVelocity + Random.Range(-verticalDispersion, verticalDispersion));
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
            
    }

    private void Explode()
    {
        //if (explosionEffect != null && Vector3.Angle(Camera.main.transform.forward, transform.position - Camera.main.transform.position) < 100f) // instead do culling
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject); // Destroy(gameObject, 3); // maybe for 3 seconds move forward spawning particle along the way
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
            return;

        Explode();
    }
}
