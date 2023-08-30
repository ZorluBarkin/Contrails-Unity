/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    private GameObject playerVehicle = null;
    private GameSettings gameSettings = null;
    private Transform flyPoint = null;

    [Header("HUD Elements")]
    [SerializeField]
    [Tooltip("Where plane player wants to go and where flypoint is, will not change if player is freelooking")]
    private GameObject crosshair = null;
    private UnityEngine.UI.Image crosshairImage = null;
    private bool crosshairTransparent = false;

    [SerializeField]
    [Tooltip("Where plane is pointing, changes shape dependent on weapon system selected if no weapon is selected it's a circle.")]
    private GameObject boreSight = null;
    private UnityEngine.UI.Image boreSightImage = null;
    private bool boreSightTransparent = false;

    [Header("Movement and Input")]
    [SerializeField]
    [Tooltip("How far the plane aims.")] 
    private float aimDistance = 800f;

    private float mouseDeltaX = 0;
    private float mouseDeltaY = 0;

    [SerializeField]
    [Tooltip("How fast mouse deltas reset to 0f.")] 
    [Range(1f, 10f)] private float lerpSpeed = 1f;
    
    private float sensitivity = 1f;

    [SerializeField]
    private bool freelook = false;

    [SerializeField]
    private Vector3 lastCursorPosition = Vector3.zero;

    private void Start()
    {
        if(gameSettings == null)
            gameSettings = GameObject.Find("Game Settings").GetComponent<GameSettings>(); // singleton

        sensitivity = gameSettings.mouseSensitivity;

        if (playerVehicle == null)
            playerVehicle = GameObject.FindGameObjectWithTag("Player"); // currently only 1 player tagged vehicle exist

        if (flyPoint == null)
            flyPoint = GameObject.Find("FlyPoint").transform; // Singleton

        if(crosshair == null)
            crosshair = transform.GetChild(0).gameObject;
        crosshairImage = crosshair.GetComponent<UnityEngine.UI.Image>();

        if (boreSight == null)
            boreSight = transform.GetChild(1).gameObject;
        boreSightImage = boreSight.GetComponent<UnityEngine.UI.Image>();

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        SetBoresight();
        SetCrosshair();
        flyPoint.position = SetFlyPoint(FreeLook());
    }

    /// <summary>
    /// Boresight Update and Setting.
    /// </summary>
    private void SetBoresight()
    {
        // make boresight change image based on weapon selected

        // make boresight not appear at rear
        if (!boreSightTransparent && Vector3.Dot(Camera.main.transform.forward, playerVehicle.transform.forward) < 0f )
        {
            boreSightTransparent = true;
            boreSightImage.color = Color.clear;
        }
        else if (boreSightTransparent && Vector3.Dot(Camera.main.transform.forward, playerVehicle.transform.forward) > 0f)
        {
            boreSightTransparent = false;
            boreSightImage.color = gameSettings.cursorColour;
        }

        // update boresight position
        boreSight.transform.position = Camera.main.WorldToScreenPoint(playerVehicle.transform.position + playerVehicle.transform.forward * aimDistance);
    }

    /// <summary>
    /// Crosshair Hiding.
    /// </summary>
    private void SetCrosshair()
    {
        if (freelook)
        {
            if (!crosshairTransparent && Vector3.Dot(Camera.main.transform.forward, flyPoint.position - playerVehicle.transform.position) < 0f)
            {
                crosshairTransparent = true;
                crosshairImage.color = Color.clear;
            }
            else if(crosshairTransparent && Vector3.Dot(Camera.main.transform.forward, flyPoint.position - playerVehicle.transform.position) > 0f)
            {
                crosshairTransparent = false;
                crosshairImage.color = gameSettings.cursorColour;
            }
        }
        else if (crosshairTransparent)
        {
            crosshairTransparent = false;
            crosshairImage.color = gameSettings.cursorColour;
        }
    }

    /// <summary>
    /// checks for input of free look, might move this to aircraft controls
    /// </summary>
    /// <returns></returns>
    private Vector3 lastVehiclePos = Vector3.zero;
    private bool FreeLook()
    {
        freelook = false;

        if (Input.GetKeyDown(KeyCode.C))
        {
            lastCursorPosition = Camera.main.transform.position + Camera.main.transform.forward * aimDistance;
            lastVehiclePos = playerVehicle.transform.position;
        }

        if (Input.GetKey(KeyCode.C))
        {
            lastCursorPosition += playerVehicle.transform.position - lastVehiclePos;
            lastVehiclePos = playerVehicle.transform.position;
            freelook = true;
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            flyPoint.position = lastCursorPosition;
            freelook = false;
        }

        return freelook;
    }

    /// <summary>
    /// Sets fly point via mouse input, freelook will cut fly algorithm, freelook here only makes crosshair not move
    /// </summary>
    private Vector3 SetFlyPoint(bool freelook = false)
    {
        if (freelook)
        {
            crosshair.transform.position = Camera.main.WorldToScreenPoint(lastCursorPosition);
            return lastCursorPosition;
        }
        else
        {
            Vector3 newPos;

            mouseDeltaX += Input.GetAxis("Mouse X") * sensitivity;
            mouseDeltaY += Input.GetAxis("Mouse Y") * sensitivity;

            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * aimDistance;

            newPos += mouseDeltaX * flyPoint.transform.right;
            newPos += mouseDeltaY * flyPoint.transform.up;
            
            crosshair.transform.position = Camera.main.WorldToScreenPoint(newPos);

            mouseDeltaX = Mathf.Lerp(mouseDeltaX, 0f, lerpSpeed * Time.deltaTime);
            mouseDeltaY = Mathf.Lerp(mouseDeltaY, 0f, lerpSpeed * Time.deltaTime);

            return newPos;
        }
    }
}
