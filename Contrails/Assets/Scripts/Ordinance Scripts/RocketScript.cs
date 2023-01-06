using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{

    private Rigidbody rb = null;
    public float initialSpeed = 0f;

    public float maxSpeed = 700f; // m/s
    public float burnTime = 1.10f;
    public float burnTimer = 0f;
    public float mass = 9.3f;

    public Vector2 blastRadius = Vector2.zero;
    public GameObject explossionEffect = null;
    public GameObject propulsionEffect = null;
    private float effectDeletionTimer = 3f;
    private float effectDeletionTime = 0f;
    private ParticleSystem.MainModule particleMain; // something empty
    private ParticleSystem.MinMaxGradient particleColor = new Color(0, 0, 0, 0);

    void Start() // add explosion and trail smoke
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * initialSpeed;
        rb.mass = mass;
    }

    void FixedUpdate() 
    {
        DoPropulsion();

        // for full simulation rotation to rockets would be needed but its unnecessary expensive and pointless
    }

    private void DoPropulsion()
    {
        if(burnTimer < burnTime)
        {
            rb.velocity += transform.forward * maxSpeed / 50;
            burnTimer += Time.deltaTime;
        }
        else
        {
            //DestroyTrailParticle();
        }
            
    }

    private void Explode()
    {
        // do Explosion

        //Destroy(gameObject);
    }

    //private void DestroyTrailParticle()
    //{
    //    if (propulsionEffect != null)
    //    {
    //        if (effectDeletionTime == 0)
    //        {
    //            propulsionEffect.transform.parent = null;
    //            particleColor = particleMain.startColor;
    //        }

    //        particleColor.color = new Color(1, 1, 1, Mathf.Clamp01((3 - effectDeletionTime) / 3));
    //        particleMain.startColor = particleColor;

    //        propulsionEffect.transform.position += propulsionEffect.transform.forward * 1f;

    //        if (effectDeletionTimer * 2f < effectDeletionTime)
    //            Destroy(propulsionEffect);

    //        effectDeletionTime += Time.deltaTime;
    //    }
    //    else
    //    {
    //        propulsionEffect = null;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
}
