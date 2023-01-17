using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameSettings : MonoBehaviour
{
    #region Camera Settings

    public int targetFrameRate = 60;

    [Range(60,120)] public int targetFOV = 90;

    public int screenNumber = 1; // Can test up to 3

    public float farViewDistance = 19312;
    private readonly float nearViewDistance = 10;

    public bool HDR = false;
    public int MSAA = 0; // 0 is off, 2x, 4x, 8x.

    #endregion

    #region HUD Variables
    public bool HUDActive = true;
    public GameObject HUD = null;

    public GameObject crosshair = null;
    public GameObject boresight = null;

    public enum HUDColour
    {
        White,
        Yellow,
        Green,
        Blue,
        Red,
        Black,
        Magenta
        //Pink
    }
    public HUDColour hudColour = HUDColour.White;

    public float crosshairSize = 0.50f;
    public float boresightSize = 0.25f;
    [Range(0, 1f)] public float hudTransparency = 1f;

    #endregion

    #region Control Variables

    // for use in other classes
    public static float _mouseSensitivity = 1f;
    public static float _cameraSpeed = 1f;

    // for setting the values
    [Range(0.1f, 10f)] public float mouseSensitivity = 1f;
    [Range(1f, 10f)] public float cameraSpeed = 5f;

    #endregion

    // textures

    // Shaders

    // VR Maybe?, i have no way to test it tho

    public bool settingsChanged = false; // APPLY button saves and applies the changes it

    // Start is called before the first frame update
    void Start()
    {
        SetGameSettings();

        if (HUD == null)
            HUD = GameObject.Find("HUD");

        if (boresight == null)
            boresight = HUD.transform.GetChild(0).gameObject;

        if (crosshair == null)
            crosshair = HUD.transform.GetChild(1).gameObject;

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
    }

    private void SetGameSettings()
    {
        // setting the sttaic variables
        _mouseSensitivity = mouseSensitivity;
        _cameraSpeed = cameraSpeed + 5;

        // Custom Application frame rate setting
        Application.targetFrameRate = targetFrameRate + 1;

        if (screenNumber == 1)
        {
            Camera.main.fieldOfView = targetFOV;
            Camera.main.farClipPlane = farViewDistance;
            Camera.main.nearClipPlane = nearViewDistance;
        }
        else if (screenNumber == 3) // look into this before doing the multi display stuff
        {

        }
    }

    private void SetHUDSettings()
    {
        HUD.SetActive(true);
        
        for(int i = 0; i < HUD.transform.childCount; i++)
        {

            //boresight.GetComponent<RectTransform>().sizeDelta = new Vector2(boresightSize, boresightSize); // makes them dissappear
            //crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(crosshairSize, crosshairSize);

            switch (hudColour)
            {
                case HUDColour.Yellow:
                    HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 0, hudTransparency);
                    break;
                case HUDColour.Green:
                    HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = new Color(0, 255, 0, hudTransparency);
                    break;
                case HUDColour.Blue:
                    HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 255, hudTransparency);
                    break;
                case HUDColour.Red:
                    HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = new Color(255, 0, 0, hudTransparency);
                    break;
                case HUDColour.Black:
                    HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, hudTransparency);
                    break;
                case HUDColour.Magenta:
                    HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 255, hudTransparency);
                    break;
                default:
                    HUD.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, hudTransparency);
                    break;
            }

        }
    
    }

}
