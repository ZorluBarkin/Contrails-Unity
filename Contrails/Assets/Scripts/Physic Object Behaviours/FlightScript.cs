using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class FlightScript : MonoBehaviour
{
    private Rigidbody rb = null;
    public float liftCoefficient = 0.5f;
    
    public float ehe = 0f;
    public AnimationCurve liftCurve = null;
    public float curveCoeff = 0f;

    public float[] airDensity = null;

    /// <summary>
    /// Used to do flight calculation for all the flying objects.
    /// </summary>
    public void FixedUpdate()
    {
        //ehe = this.gameObject.transform.rotation.x;
        //liftCoefficient = CalculateLiftCoefficient(-1 * this.gameObject.transform.rotation.x); // at 1 should be max
        curveCoeff = liftCurve.Evaluate(90 - (this.gameObject.transform.rotation.eulerAngles.x % 90));

    }
    
    /// <summary>
    /// This is a modified SmoothstepCurve for getting the lift coefficient.
    /// </summary>
    /// <param name="angleOfAttack"></param>
    /// <returns></returns>
    //private float CalculateLiftCoefficient(float angleOfAttack) //
    //{
    //    float coeff = 0.5f;

    //    if (angleOfAttack > 0)
    //        coeff = 3.2f * (angleOfAttack * angleOfAttack) - 2 * (angleOfAttack * angleOfAttack * angleOfAttack) + 0.5f;
    //    else
    //        coeff = 2 * angleOfAttack + 0.5f;

    //    return coeff;
    //}
}
