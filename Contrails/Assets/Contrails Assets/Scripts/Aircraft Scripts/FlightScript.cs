/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class FlightScript : MonoBehaviour
{
    // lift modes
    public FlightMode flightMode = FlightMode.Realistic;

    public AircraftControls aircraftControls = null;
    private Rigidbody rb = null;
    private WeatherScript weather = null;

    #region Maneuverability Variables
    public AnimationCurve liftCurve = null;
    public float LiftCoefficient = 0f;
    public float wingArea = 28f; // m^2 // first generation of f-16 has 26 m^2 wing area
    public float MaxSpeed = 0f; // might not need
    public float stallSpeed = 85f; // m/s // may not need
    public float thrust = 4218.41f;

    [Header("Input")]
    [SerializeField]
    private Transform flyPoint = null;

    [Range(-1f, 1f)] private float pitch = 0f;
    private bool pitchOverride = false;
    private float calculatedPitch = 0f;

    [Range(-1f, 1f)] private float roll = 0f;
    private bool rollOverride = false;
    private float calculatedRoll = 0f;

    [Range(-1f, 1f)] private float yaw = 0f;
    private bool yawOverride = false;
    private float calculatedYaw = 0f;

    //public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    //public float Yaw { set { yaw = Mathf.Clamp(value, -1f, 1f); } get { return yaw; } }
    //public float Roll { set { roll = Mathf.Clamp(value, -1f, 1f); } get { return roll; } }

    //[Header("Not implemented")]
    //public MouseFlightController mouseFlightController;

    //public float correctionSensitivity = 5f;
    //public float startRollDegree = 5f;

    [Tooltip("Pitch, Roll, Yaw")]
    public Vector3 turnTorque = new Vector3(30f, 15f, 5f);
    //private float forceMultiplier = 0.1f;

    public static float _speed = 0f; // this may stay static
    #endregion

    #region Control Variables
    public float throttle = 1f; // if 100% its 1 else lower if WEP go for 1.05 - 1.10 etc.

    #endregion

    public float lift = 0f; // make it private wehn test are done

    private bool arcadeSet = false;

    private void Start() // to check the method for lift calclulation.
    {
        if(weather == null)
            weather = FindObjectOfType<WeatherScript>();

        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if (flyPoint == null)
            flyPoint = GameObject.Find("FlyPoint").transform;

        throttle = 0f;

        //flightMode = GameSettings._flightMode;

        //if(flightMode == FlightMode.SimpleLift)
        //    rb.useGravity = false;
    }

    //float autoYaw = 0f, autoPitch = 0f, autoRoll = 0f;
    private void Update()
    {
        GetFlightInput();

        FlyToPoint(flyPoint);

        pitchOverride = false;
        rollOverride = false;
        yawOverride = false;
    }

    /// <summary>
    /// Used to do flight calculation for all the flying objects.
    /// </summary>
    public void FixedUpdate()
    {
        _speed = rb.velocity.magnitude;

        rb.AddForce(transform.forward * thrust * throttle, ForceMode.Force);

        rb.AddRelativeTorque(new Vector3(turnTorque.x * calculatedPitch, turnTorque.z * calculatedYaw, turnTorque.y * calculatedRoll), ForceMode.Force);

        calculatedPitch = 0;
        calculatedRoll = 0;
        calculatedYaw = 0;

        //rb.AddRelativeTorque(new Vector3(turnTorque.x * pitch,
        //                                    turnTorque.y * yaw,
        //                                    -turnTorque.z * roll) * forceMultiplier,
        //                        ForceMode.Force);

        #region Old, Closed Flight Mode
        //switch (flightMode)
        //{
        //    case FlightMode.SimpleLift:
        //        if (rb.velocity.magnitude < 100 && AAMScript.burnTimer > AAMScript.burnTime)
        //        {
        //            rb.useGravity = true;
        //            flightMode = FlightMode.Realistic;
        //        }
        //        break;
        //    case FlightMode.Arcade: // Flight does not work good, like almost not at all
        //        if (!arcadeSet)
        //        {
        //            rb.useGravity = false;
        //            arcadeSet = true;
        //        }
        //        rb.AddRelativeForce(transform.forward * thrust * throttle, ForceMode.Force);
        //        rb.AddRelativeTorque(new Vector3(turnTorque.x * aircraftControls.pitch, turnTorque.y * aircraftControls.yaw, turnTorque.z * aircraftControls.roll) * forceMultiplier, ForceMode.Acceleration);
        //        break;
        //    default: // deafault is realistic
        //      //lift = LiftCoefficient * (weather.airDensity[GetAltitudeIndex()] * rb.velocity.magnitude * rb.velocity.magnitude / 2) * wingArea;
        //      // Get lift coefficient
        //      LiftCoefficient = liftCurve.Evaluate(Vector3.Angle(Vector3.forward, transform.forward));
        //      // Calculate lift
        //      lift = weather.airDensity[GetAltitudeIndex()];
        //      rb.AddForce(transform.up * LiftCoefficient * (weather.airDensity[GetAltitudeIndex()] * rb.velocity.magnitude * rb.velocity.magnitude / 2) * wingArea); // too big need to understand why
        //        break;
        //}
        #endregion
    }

    private void FlyToPoint(Transform flyPoint)
    {
        // fly to target

        if(pitchOverride)
            calculatedPitch = pitch;
        
        if(rollOverride)
            calculatedRoll = roll;

        if(yawOverride)
            calculatedYaw = yaw;
    }

    /// <summary>
    /// Reads input from user via keyboard.
    /// </summary>
    private void GetFlightInput()
    {
        // Throttle Input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle += 0.5f * Time.deltaTime; // add sensitivity * ThrottleSensitivity
            throttle = Mathf.Clamp01(throttle);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle -= 0.5f * Time.deltaTime; // add sensitivity * ThrottleSensitivity
            throttle = Mathf.Clamp01(throttle);
        }

        // pitching
        if (Input.GetKey(KeyCode.W))
        {
            // pitch down
            pitch = 1;
            pitchOverride = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // pitch up
            pitch = -1;
            pitchOverride = true;
        }

        // rolling
        if (Input.GetKey(KeyCode.A))
        {
            // roll left
            roll = 1;
            rollOverride = true;
        }
        else if (Input.GetKey(KeyCode.D)) // currently priotirized may chnage later to cancel out
        {
            // roll right
            roll = -1;
            rollOverride = true;
        }
        
        // yaw input
        if (Input.GetKey(KeyCode.Q))
        {
            // yaw left
            yaw = -1;
            yawOverride = true;
        }
        else if (Input.GetKey(KeyCode.E)) // currently priotirized may chnage later to cancel out
        {
            // yaw right
            yaw = 1;
            yawOverride = true;
        }

    }

    /// <summary>
    /// Returns the index of the array of airDensity values
    /// </summary>
    /// <returns></returns>
    private int GetAltitudeIndex()
    {
        float altitude = this.gameObject.transform.position.y;

        if (altitude <= 500) // this is not good coding but its fast for air Density array system (array)
            return 0;
        else if (altitude <= 1500)
            return 1;
        else if (altitude <= 2500)
            return 2;
        else if (altitude <= 3500)
            return 3;
        else if (altitude <= 4500)
            return 4;
        else if (altitude <= 5500)
            return 5;
        else if (altitude <= 6500)
            return 6;
        else if (altitude <= 7500)
            return 7;
        else if (altitude <= 8500)
            return 8;
        else if (altitude <= 9500)
            return 9;
        else if (altitude <= 12500)
            return 10;
        else if (altitude <= 17500)
            return 11;
        else
            return 12;
    }

    /// <summary>
    /// This is a modified SmoothstepCurve for getting the lift coefficient.
    /// </summary>
    /// <param name="angleOfAttack"></param>
    /// <returns></returns>
    private float CalculateLiftCoefficient(float angleOfAttack) //
    {
        float coeff = 0.5f;

        if (angleOfAttack > 0)
            coeff = 3.2f * (angleOfAttack * angleOfAttack) - 2 * (angleOfAttack * angleOfAttack * angleOfAttack) + 0.5f;
        else
            coeff = 2 * angleOfAttack + 0.5f;

        return coeff;
    }
}
