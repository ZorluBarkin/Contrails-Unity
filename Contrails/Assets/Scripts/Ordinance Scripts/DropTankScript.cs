using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropTankScript : MonoBehaviour
{
    public Rigidbody rb = null;
    public AircraftControls aircraftControls = null;
    public float tankWidth = 0f;

    public float fuel = 0f; // in Litres
    public float emptyWeight = 0f; // in kg
    public float fullWeight = 0f;

    private float fuelUse = 0f; // get from aircraft
    public bool inUse = false;
    public int numberOfTanksInUse = 1;
    public bool jettison = false;
    private bool jettisoned = false;
    public float speed = 0f; // m/s from the airplane

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (aircraftControls == null)
            transform.parent.parent.GetComponent<AircraftControls>();

        rb.useGravity = false;
        fullWeight = emptyWeight + fuel * 0.870f; // density of military grade jet fuel is 870 g / m^3 max.
        transform.position -= transform.up * (tankWidth / 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (jettison)
        {
            jettison = false;
            jettisoned = true;
            transform.parent = null;
            rb.useGravity = true;
            rb.velocity = transform.forward * speed;
        }

        if (jettisoned)
        {
            transform.Rotate(1.5f, 0, 0);

            if (transform.position.y < 5f)
                Destroy(this.gameObject);

            return;
        }
        

        if (fuel <= 0)
            return;

        if (inUse)
            fuel -= fuelUse * aircraftControls.throttle / numberOfTanksInUse;

    }
}
