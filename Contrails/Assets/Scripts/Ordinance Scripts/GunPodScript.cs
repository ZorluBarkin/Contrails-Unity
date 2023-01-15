using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPodScript : MonoBehaviour
{
    // no jettison for gunpods

    public bool armed = false;
    public bool fire = false;
   
    private bool empty = false;
    public int totalAmmo = 0;
    private int calculatedAmmo = 0;
    private int initialAmmo = -1;

    public bool fireRateChangeable = false;
    public float fireRate = 1000f;

    public float weight = 630.50f; // kg
    public float maxWeight = 630.50f;
    public float emptyWeight = 357f;

    private List<GunScript> guns = new List<GunScript>();

    // Start is called before the first frame update
    void Start()
    {
        if(guns.Count == 0)
        {
            for (int i = 1; i < transform.childCount; i++) // first one is always will be the model
            {
                guns.Add(transform.GetChild(i).GetComponent<GunScript>());
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (empty)
            return;

        for (int i = 0; i < guns.Count; i++)
        {
            calculatedAmmo += guns[i].ammoCount;
            guns[i].armed = armed;

            if (fireRateChangeable)
            {
                guns[i].fireRate = fireRate;
                guns[i].fireRateChanged = true;
            }

        }

        if(totalAmmo != calculatedAmmo)
            totalAmmo = calculatedAmmo;

        calculatedAmmo = 0;

        if (initialAmmo == -1)
            initialAmmo = totalAmmo;

        if (totalAmmo <= 0)
        {
            empty = true;
            totalAmmo = 0;
            weight = emptyWeight;
            armed = false;
            fire = false;
        }

        if (!armed)
            return;

        if (fire)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                guns[i].fire = true;
            }
            
            weight = emptyWeight + (maxWeight - emptyWeight) * totalAmmo / initialAmmo;
        }

        
    }
}
