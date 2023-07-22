/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody rb = null;
    public GameObject explosionEffect = null;

    public GameObject poolParent = null;

    [HideInInspector] public bool shot = false;
    private bool shotOut = false;

    public float muzzleVelocity = 1050f;
    public float lifeTime = 10; // 10 seconds was default
    private float lifeTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
            rb = transform.GetComponent<Rigidbody>();

        if (!GunScript._bulletsArePooled)
            rb.velocity = transform.forward * muzzleVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime < lifeTimer)
        {
            if (GunScript._bulletsArePooled)
            {
                lifeTimer = 0;
                shot = false;
                shotOut = false;
                rb.velocity = Vector3.zero;
                transform.position = new Vector3(0, -100, 0);
                //gameObject.transform.parent = poolParent.transform;
                gameObject.SetActive(false);
            }
            else
                Destroy(gameObject);
        }

        if (shot)
        {
            if (!shotOut)
            {
                shotOut = true;
                rb.velocity = transform.forward * muzzleVelocity;
            }

            lifeTimer += Time.deltaTime;
        }


    }

    private void Explode()
    {
        // if object pooling is here do not destroy
        if (GunScript._bulletsArePooled)
        {
            shot = false;
            shotOut = false;
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // do damage

            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0, -100, 0);
            //gameObject.transform.parent = poolParent.transform;
            gameObject.SetActive(false);
        }
        else
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            
            // do damage

            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            return;
        }

        Explode();
    }
}
