/*  
 * Copyright 2023 Barkın Zorlu 
 * All rights reserved.
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/4.0/ or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraScript : MonoBehaviour
{
    public GameObject playerVehicle = null;
    public GameObject HUD = null;
    private Camera cam = null;
    public AircraftControls aircraftControls = null;
    private bool isAircraft = false;
    public GameObject cube = null;
    #region HUD Variables

    public float boresightDistance = 800f;
    private Vector3 boresightPosition = Vector3.zero;
    public RectTransform boresight = null;

    public RectTransform crosshair = null;

    private /*static*/ Quaternion turnTo = Quaternion.identity;

    #endregion

    private void Start()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        if ( playerVehicle == null ) // default position
        {
            transform.position = new Vector3(0, 2000, 0);
            transform.rotation = Quaternion.identity;
        }

        if (HUD == null)
            HUD = GameObject.Find("HUD");

        if (aircraftControls == null)
            isAircraft = playerVehicle.TryGetComponent<AircraftControls>(out aircraftControls);

        if (!isAircraft)
            Debug.Log("Does not have AircraftControls");

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Stay behind the plane
        if (playerVehicle != null)
        {
            MouseFlight();
        }

    }

    private void MouseFlight()
    {
        boresightPosition = playerVehicle.transform.position;
        boresightPosition += playerVehicle.transform.forward * boresightDistance;

        Vector3 position = playerVehicle.transform.position - transform.forward * 25 + transform.up * 7.5f;
        transform.position = Vector3.Lerp(transform.position, position, GameSettings._cameraSpeed * Time.deltaTime);
        

        boresight.position = cam.WorldToScreenPoint(boresightPosition);
        crosshair.position = Input.mousePosition;

        //Vector3 reachTo = new Vector3(transform.position.x + Input.mousePosition.x - Screen.width / 2, transform.position.y + Input.mousePosition.y - Screen.height / 2, transform.position.z + 200);
        turnTo = Quaternion.Euler(transform.rotation.eulerAngles.x + aircraftControls.mouseY, transform.rotation.eulerAngles.y + aircraftControls.mouseX, 0);

        //cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, turnTo, 100f * Time.deltaTime); //turnTo;

        cam.transform.rotation = Quaternion.Slerp(transform.rotation, turnTo, 100f * Time.deltaTime);

        //cam.transform.rotation = Quaternion.Slerp(transform.rotation, turnTo, 5f * Time.deltaTime);

        // lock the camera to horizon
        cam.transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, 0);
    }

}
