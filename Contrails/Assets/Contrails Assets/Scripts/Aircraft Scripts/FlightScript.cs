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
    [SerializeField][Range(-1f, 1f)] private float pitch = 0f;
    [SerializeField][Range(-1f, 1f)] private float yaw = 0f;
    [SerializeField][Range(-1f, 1f)] private float roll = 0f;

    public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    public float Yaw { set { yaw = Mathf.Clamp(value, -1f, 1f); } get { return yaw; } }
    public float Roll { set { roll = Mathf.Clamp(value, -1f, 1f); } get { return roll; } }

    //[Header("Not implemented")]
    //public MouseFlightController mouseFlightController;

    public float correctionSensitivity = 5f;
    public float startRollDegree = 5f;

    public Vector3 turnTorque = new Vector3(90f, 45f, 25f);
    private float forceMultiplier = 0.1f;

    public static float _speed = 0f;
    #endregion

    #region Control Variables
    private float throttle = 1f; // if 100% its 1 else lower if WEP go for 1.05 - 1.10 etc.

    #endregion

    public float lift = 0f; // make it private wehn test are done

    private bool arcadeSet = false;

    private void Start() // to check the method for lift calclulation.
    {
        if(weather == null)
            weather = FindObjectOfType<WeatherScript>();

        if(rb == null)
            rb = GetComponent<Rigidbody>();

        //flightMode = GameSettings._flightMode;

        if(flightMode == FlightMode.SimpleLift)
            rb.useGravity = false;
    }

    float autoYaw = 0f, autoPitch = 0f, autoRoll = 0f;
    private void Update()
    {
        //MouseFlight(mouseFlightController.MouseAimPos, out autoYaw, out autoPitch, out autoRoll);

        yaw = autoYaw;
        pitch = autoPitch;
        roll = autoRoll;

        //yaw = autoYaw;
        //pitch = (pitchOverride) ? keyboardPitch : autoPitch;
        //roll = (rollOverride) ? keyboardRoll : autoRoll;
    }

    /// <summary>
    /// Used to do flight calculation for all the flying objects.
    /// </summary>
    public void FixedUpdate()
    {
        throttle = aircraftControls.throttle;
        _speed = rb.velocity.magnitude;

        rb.AddRelativeForce(Vector3.forward * thrust * forceMultiplier, ForceMode.Force);
        rb.AddRelativeTorque(new Vector3(turnTorque.x * pitch,
                                            turnTorque.y * yaw,
                                            -turnTorque.z * roll) * forceMultiplier,
                                ForceMode.Force);

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

    private void MouseFlight(Vector3 flyTarget, out float yaw, out float pitch, out float roll)
    {
        // This is my usual trick of converting the fly to position to local space.
        // You can derive a lot of information from where the target is relative to self.
        Vector3 localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized * correctionSensitivity;
        float angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);

        // IMPORTANT!
        // These inputs are created proportionally. This means it can be prone to
        // overshooting. The physics in this example are tweaked so that it's not a big
        // issue, but in something with different or more realistic physics this might
        // not be the case. Use of a PID controller for each axis is highly recommended.

        // ====================
        // PITCH AND YAW
        // ====================

        // Yaw/Pitch into the target so as to put it directly in front of the aircraft.
        // A target is directly in front the aircraft if the relative X and Y are both
        // zero. Note this does not handle for the case where the target is directly behind.
        yaw = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1f, 1f);

        // ====================
        // ROLL
        // ====================

        // Roll is a little special because there are two different roll commands depending
        // on the situation. When the target is off axis, then the plane should roll into it.
        // When the target is directly in front, the plane should fly wings level.

        // An "aggressive roll" is input such that the aircraft rolls into the target so
        // that pitching up (handled above) will put the nose onto the target. This is
        // done by rolling such that the X component of the target's position is zeroed.
        var agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);

        // A "wings level roll" is a roll commands the aircraft to fly wings level.
        // This can be done by zeroing out the Y component of the aircraft's right.
        var wingsLevelRoll = transform.right.y;

        // Blend between auto level and banking into the target.
        var wingsLevelInfluence = Mathf.InverseLerp(0f, startRollDegree, angleOffTarget);
        roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
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
