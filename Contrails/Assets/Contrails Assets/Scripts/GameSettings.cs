/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum FlightMode
{
    SimpleLift,
    Arcade,
    Realistic
}

public class GameSettings : MonoBehaviour
{
    #region Camera Settings
    [Header("Graphic Settings")]
    public int targetFrameRate = 60;

    [Range(60,120)] public int targetFOV = 90;

    public int screenNumber = 1; // Can test up to 3

    public float farViewDistance = 19312;
    private readonly float nearViewDistance = 10;

    public bool HDR = false;
    public int MSAA = 0; // 0 is off, 2x, 4x, 8x.

    #endregion

    #region HUD Variables
    [Header("HUD Variables")]
    [SerializeField]
    private bool HUDActive = true;

    public GameObject HUD = null;
    public GameObject crosshair = null;
    public GameObject boresight = null;

    [SerializeField] private Color cursorColour = Color.white;
    [SerializeField] private Color HUDColour = Color.white;

    [SerializeField]
    [Range(10f, 100f)] private float crosshairSize = 50f;
    
    [SerializeField]
    [Range(5, 50f)] private float boresightSize = 25f;

    #endregion

    #region Control Variables
    [Header("Control Settings")]

    public TextMeshProUGUI weaponSelectionTextMesh = null;
    [Range(0.1f, 10f)] public float mouseSensitivity = 1f;

    #endregion

    #region Game Settings
    [Header("Gameplay Settings")]
    //public static FlightMode _flightMode = FlightMode.Realistic;
    [SerializeField] FlightMode flightMode = FlightMode.Arcade; // setting it for now

    // selection material colours
    [SerializeField]
    [Tooltip("Pylon Selection Material")]
    private Material selectionMat = null;
    
    [SerializeField]
    [Tooltip("Pylon Selected Material")]
    private Material selectedMat = null;
    
    [SerializeField]
    [Tooltip("Pylon Applied Material")]
    private Material appliedMat = null;

    public bool bulletsArePooled = false;
    #endregion
    // textures

    // Shaders

    // VR Maybe?, i have no way to test it tho
    [Space(10)]
    public bool settingsChanged = false; // APPLY button saves and applies the changes

    private void Awake()
    {
        GunScript._bulletsArePooled = bulletsArePooled;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (HUD == null)
            HUD = GameObject.Find("HUD").gameObject; // singleton

        if (boresight == null)
            boresight = HUD.transform.GetChild(1).gameObject;

        if (crosshair == null)
            crosshair = HUD.transform.GetChild(0).gameObject;

        if (weaponSelectionTextMesh == null)
            weaponSelectionTextMesh = HUD.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        SetGameSettings();

        SetHUDSettings();
        //Debug.Log("displays connected: " + Display.displays.Length);
    }

    private void LateUpdate()
    {
        if (settingsChanged)
        {
            SetGameSettings();

            if (HUDActive)
                SetHUDSettings();
            else
                HUD.SetActive(false);

            settingsChanged = false;
        }

        // wrong place curretnly here for debug
        weaponSelectionTextMesh.text = AircraftControls._weaponSelectionString; // add drop Series too
    }

    private void SetGameSettings()
    {
        // Custom Application frame rate setting
        Application.targetFrameRate = targetFrameRate + 1;

        //if (screenNumber == 1)
        //{
        //    Camera.main.fieldOfView = targetFOV; 
        //    Camera.main.farClipPlane = farViewDistance;
        //    Camera.main.nearClipPlane = nearViewDistance;
        //}
        //else if (screenNumber == 3) // look into this before doing the multi display stuff
        //{

        //}
    }

    private void SetHUDSettings()
    {
        HUD.SetActive(true);
        
        for(int i = 0; i < 2; i++)
        {
            crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(crosshairSize, crosshairSize);
            boresight.GetComponent<RectTransform>().sizeDelta = new Vector2(boresightSize, boresightSize); // makes them dissappear

            HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = cursorColour;
        }
        
        // text mesh Colours
        for(int i = 2; i < HUD.transform.childCount; i++) // starts after 2 because this is after cursor settings
        {
            HUD.transform.GetChild(i).GetComponent<TextMeshProUGUI>().color = HUDColour;
        }

    }

}
