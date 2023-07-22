/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public static bool _bulletsArePooled = true;
    public GameObject[] bulletArray = null;
    public BulletScript[] bulletScriptArray = null;
    public GameObject poolParent = null;
    public GameObject bullet = null;

    public GameObject muzzleSmoke = null;

    public bool armed = false;
    public bool fire = false;
    private bool empty = false;

    public float fireRate = 1000f; // rpm need to turn to rps (divide by 60)
    public bool fireRateChanged = false;
    public float deviation = 2f; // deviation in degrees
    //private bool spawnTwo = false;
    //private bool sequential = false; // one time spawn 2 one time 1 maybe // 4500rpm // dont work
    //private int oscillatoryNumber = 0;

    //public float warmUpTime = 0f; // this is viable for rotary cannons // did not implement yet need to increase firerate untill warmup reached
    //private float warmUpTimer = 0f;

    private float fireTimer = 0f;
    public int ammoCount = 100;
    public float tracerRate = 5f; // how many rounds between tracers ex: 5; N N N N T - N N N N T

    private void Start()
    {
        int poolSize = Mathf.RoundToInt(fireRate / 60 * bullet.GetComponent<BulletScript>().lifeTime + fireRate / 1000 * 1);

        if (_bulletsArePooled)
        {
            bulletArray = new GameObject[poolSize];
            bulletScriptArray = new BulletScript[poolSize];
            Vector3 poolPos = new Vector3(0, -100, 0);

            for (int i = 0; i < poolSize; i++)
            {
                bulletArray[i] = Instantiate(bullet, poolPos, Quaternion.identity, poolParent.transform);
                bulletScriptArray[i] = bulletArray[i].GetComponent<BulletScript>();
                bulletScriptArray[i].poolParent = poolParent;
                bulletArray[i].SetActive(false);
            }
                
        }

        //setFireRate();
    }

    void FixedUpdate()
    {
        if (empty)
            return;

        if (fireRateChanged)
        {
            //setFireRate();
            fireRateChanged = false;
        }

        if (armed && fire)
        {

            Fire(_bulletsArePooled);

            if (ammoCount <= 0)
            {
                empty = true;
                ammoCount = 0;
                armed = false;
                fire = false;
            }

        }
    }

    private void Fire(bool objectPooling = false)
    {
        #region commented out
        //if (spawnTwo)
        //{
        //    if (sequential)
        //    {
        //        if (oscillatoryNumber % 2 > 0)
        //        {
        //            if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
        //            {
        //                if (objectPooling)
        //                {
        //                    // TODO: MAKE IT SHOOT DOUBLE
        //                    for(int i = 0; i < bulletArray.Length; i++)
        //                    {
        //                        if (!bulletArray[i].activeSelf)
        //                        {
        //                            bulletArray[i].transform.position = transform.position;
        //                            bulletArray[i].transform.rotation = transform.rotation;
        //                            //bulletArray[i].transform.parent = null;
        //                            bulletArray[i].SetActive(true);
        //                            bulletScriptArray[i].shot = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Instantiate<GameObject>(bullet, transform.position + transform.forward * 6.5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                    Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
        //                    Instantiate<GameObject>(bullet, transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                    Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
        //                }

        //                ammoCount -= 2;
        //                fireTimer = 0;
        //            }
        //            fireTimer += Time.deltaTime;
        //        }
        //        else
        //        {
        //            if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
        //            {
        //                if (objectPooling)
        //                {
        //                    for (int i = 0; i < bulletArray.Length; i++)
        //                    {
        //                        if (!bulletArray[i].activeSelf)
        //                        {
        //                            //bulletArray[i].transform.parent = null;
        //                            bulletArray[i].SetActive(true);
        //                            bulletArray[i].transform.position = transform.position + transform.forward * 5f;
        //                            bulletArray[i].transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                                Random.Range(-deviation, deviation), Random.Range(-deviation, deviation));
        //                            bulletScriptArray[i].shot = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Instantiate<GameObject>(bullet, transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                    Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
        //                }


        //                ammoCount--;
        //                fireTimer = 0;
        //            }
        //            fireTimer += Time.deltaTime;
        //        }

        //        oscillatoryNumber++;
        //    }
        //    else
        //    {
        //        if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
        //        {
        //            if (objectPooling)
        //            {
        //                for (int i = 0; i < bulletArray.Length; i++)
        //                {   // TODO: MAKE IT SHOOT DOUBLE
        //                    if (!bulletArray[i].activeSelf)
        //                    {
        //                        //bulletArray[i].transform.parent = null;
        //                        bulletArray[i].SetActive(true);
        //                        bulletArray[i].transform.position = transform.position + transform.forward * 5f;
        //                        bulletArray[i].transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                            Random.Range(-deviation, deviation), Random.Range(-deviation, deviation));
        //                        bulletScriptArray[i].shot = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Instantiate<GameObject>(bullet, transform.position + transform.forward * 6.5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
        //                Instantiate<GameObject>(bullet, transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
        //            }

        //            ammoCount -= 2;
        //            fireTimer = 0;
        //        }
        //        fireTimer += Time.deltaTime;
        //    }

        //}
        //else // firing less than 3000
        //{
        //    if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
        //    {
        //        if (objectPooling)
        //        {
        //            for (int i = 0; i < bulletArray.Length; i++)
        //            {
        //                if (!bulletArray[i].activeSelf)
        //                {
        //                    //bulletArray[i].transform.parent = null; // may not need 
        //                    bulletArray[i].SetActive(true);
        //                    bulletArray[i].transform.position = transform.position + transform.forward * 5f;

        //                    // bulllets rotation switches 50.005 and -62.08 on x axis dont know why
        //                    bulletArray[i].transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //                        Random.Range(-deviation, deviation), Random.Range(-deviation, deviation));
        //                    bulletScriptArray[i].shot = true;
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Instantiate<GameObject>(bullet, transform.position + transform.forward * 5f, transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
        //            Random.Range(-deviation, deviation), Random.Range(-deviation, deviation)));
        //        }

        //        ammoCount--;
        //        fireTimer = 0;
        //    }
        //    fireTimer += Time.deltaTime;
        //}
        #endregion

        if (fireTimer > 1f / (fireRate / (1f / Time.deltaTime)))
        {
            for (int i = 0; i < bulletArray.Length; i++)
            {
                if (!bulletArray[i].activeSelf)
                {
                    bulletArray[i].transform.position = transform.position + transform.forward * 5f;
                    bulletArray[i].transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(-deviation, deviation),
                        Random.Range(-deviation, deviation), Random.Range(-deviation, deviation));bulletArray[i].SetActive(true);
                    bulletScriptArray[i].shot = true;
                    break;
                }
            }
            ammoCount--;
            fireTimer = 0;
        }
        fireTimer += Time.deltaTime;
    }

    //private void setFireRate()
    //{
    //    if (fireRate > 4500)
    //    {
    //        spawnTwo = true;
    //        sequential = false;
    //    }
    //    else if (fireRate > 3000 && fireRate < 4500)
    //    {
    //        spawnTwo = true;
    //        sequential = true;
    //    }
    //}
}
