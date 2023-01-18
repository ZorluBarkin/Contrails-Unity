using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody rb = null;
    public GameObject explosionEffect = null;

    public float muzzleVelocity = 1050f;
    private float lifeTime = 10;
    private float lifeTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
            rb = transform.GetComponent<Rigidbody>();

        rb.velocity = transform.forward * muzzleVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime < lifeTimer)
        {
            Destroy(gameObject);
        }

        lifeTimer += Time.deltaTime;
    }

    private void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // do damage
        // Debug.Log("Hello World!");

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            return;
        }

        Explode();
    }
}
