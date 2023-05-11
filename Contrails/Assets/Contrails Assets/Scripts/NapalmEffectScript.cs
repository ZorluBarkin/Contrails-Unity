using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmEffectScript : MonoBehaviour
{
    public GameObject particleEffect = null;
    //public float rollingDistance = 90f;

    private float burnTimer = 0f;
    [HideInInspector] public float burnTime = 120f; // seconds
    [HideInInspector] public Vector2 burnTemperature = new Vector2(900f, 1300f);
    private float DPS = 0f;

    public float reachHeight = 15;
    private bool spawnedInitial = false;
    private bool spawningDone = false;
    private bool spawned = false;
    private float rolledTime = 0f;

    void Start()
    {
        if(particleEffect == null)
            particleEffect = transform.GetChild(0).gameObject;

        DPS = (burnTemperature.x + burnTemperature.y) / 2 * 0.02f; // for now, NP-1 about 110DPS

        DOTScript childDOT = particleEffect.GetComponent<DOTScript>();

        childDOT.duration = burnTime;
        childDOT.damage = DPS;

        SpawnFire();

        spawnedInitial = true;
    }

    void Update()
    {
        if (spawningDone)
        {
            if (burnTime < burnTimer)
                Destroy(gameObject);
            else
                burnTimer += Time.deltaTime;

            return;
        }
        
        if (spawnedInitial && rolledTime > 0.5f && !spawned)
        {
            Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y + reachHeight, transform.position.z);

            RaycastHit hit;
            if (Physics.Raycast(raycastPosition, -transform.up, out hit, reachHeight, LayerMask.GetMask("Ground")))
            {
                Debug.Log(hit.point.y);
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }

            spawned = true;
            SpawnFire();

        }


        if(rolledTime < 1f)
        {
            transform.position += transform.forward * 90 * Time.deltaTime;
            rolledTime += Time.deltaTime;
        }
        else 
        {
            spawningDone = true;

            Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            
            RaycastHit hit;
            if (Physics.Raycast(raycastPosition, -transform.up, out hit, reachHeight + 5f, LayerMask.GetMask("Ground")))
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            else
                Destroy(gameObject);
                
        }

    }

    private void SpawnFire()
    {
        GameObject go = Instantiate(particleEffect, transform.position, Quaternion.identity);
        DOTScript DOT = go.GetComponent<DOTScript>();
        
        DOT.damage = DPS;
        DOT.duration = burnTime;
    }
}
