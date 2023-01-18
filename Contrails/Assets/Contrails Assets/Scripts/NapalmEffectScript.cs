using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmEffectScript : MonoBehaviour
{
    public GameObject particleEffect = null;
    public float burnTime = 120f; // seconds
    private float burnTimer = 0f;
    public float rollingDistance = 90f;
    public Vector2 burnTemperature = new Vector2(900f, 1300f);
    private float DPS = 0f;
    
    private bool spawnedInitial = false;
    private bool spawningDone = false;
    private bool spawned = false;
    private float rolledTime = 0f;

    void Start()
    {
        if(particleEffect == null)
            particleEffect = transform.GetChild(0).gameObject;

        DPS = burnTemperature.x + burnTemperature.y / 2 / 10 * 0.02f; // for now, NP-1 about 110DPS

        Instantiate(particleEffect, transform.position, Quaternion.identity);
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
            spawned = true;
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }


        if(rolledTime < 1f)
        {
            transform.position += transform.forward * 90 * Time.deltaTime;
            rolledTime += Time.deltaTime;
        }
        else
            spawningDone = true;

    }

    private void OnTriggerStay(Collider other)
    {
        //float hitHeatlth = other.gameObject.TryGetComponent</*CombatantScript???*/>().health -= DPS;

        //if (hitHeatlth <= 0)
        //    Destroy(other.gameObject);
    }
}
