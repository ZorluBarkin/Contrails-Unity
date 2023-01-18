using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject bullet = null;
    public GameObject muzzleSmoke = null;

    public bool armed = false;
    public bool fire = false;
    private bool empty = false;

    public float fireRate = 1000f; // rpm need to turn to rps (divide by 60)
    public bool fireRateChanged = false;
    public float deviation = 2f; // deviation in degrees
    private bool spawnTwo = false;
    private bool sequential = false; // one time spawn 2 one time 1 maybe // 4500rpm // dont work
    private int oscillatoryNumber = 0;

    //public float warmUpTime = 0f; // this is viable for rotary cannons // did not implement yet need to increase firerate untill warmup reached
    //private float warmUpTimer = 0f;

    private float fireTimer = 0f;
    public int ammoCount = 100;
    public float tracerRate = 5f; // how many rounds between tracers ex: 5; N N N N T - N N N N T

    private void Start()
    {
        setFireRate();
    }

    void FixedUpdate()
    {
        if (empty)
            return;

        if (fireRateChanged)
        {
            setFireRate();
            fireRateChanged = false;
        }

        if (armed && fire)
        {

            Fire();
            
            if(ammoCount <= 0)
            {
                empty = true;
                ammoCount = 0;
                armed= false;
                fire = false;
            }

        }
    }

    private void Fire() // redo with max rpm at 2500, test with different computers decide on max after the results.
    {

        if (spawnTwo)
        {
            if (sequential)
            {
                if (oscillatoryNumber % 2 > 0)
                {
                    if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
                    {
                        Instantiate<GameObject>(bullet,transform.position + transform.forward * 6.5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
                        Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
                        Instantiate<GameObject>(bullet,transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
                        Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));

                        ammoCount -= 2;
                        fireTimer = 0;
                    }
                    fireTimer += Time.deltaTime;
                }
                else
                {
                    if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
                    {
                        Instantiate<GameObject>(bullet, transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
                        Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));

                        ammoCount--;
                        fireTimer = 0;
                    }
                    fireTimer += Time.deltaTime;
                }

                oscillatoryNumber++;
            }
            else
            {
                if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
                {
                    Instantiate<GameObject>(bullet, transform.position + transform.forward * 6.5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation), 
                        Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
                    Instantiate<GameObject>(bullet, transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation), 
                        Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));

                    ammoCount -= 2;
                    fireTimer = 0;
                }
                fireTimer += Time.deltaTime;
            }

        }
        else
        {
            if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
            {
                Instantiate<GameObject>(bullet, transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation), 
                    Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));

                ammoCount--;
                fireTimer = 0;
            }
            fireTimer += Time.deltaTime;
        }
        
    }

    private void setFireRate()
    {
        if (fireRate > 4500)
        {
            spawnTwo = true;
            sequential = false;
        }
        else if (fireRate > 3750)
        {
            spawnTwo = true;
            sequential = true;
        }
    }
}
