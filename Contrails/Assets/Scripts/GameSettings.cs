using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int targetFrameRate = 61;

    public int targetFOV = 60;

    public bool settingsChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        SetGameSettings();
    }

    void FixedUpdate() // fixed update used for less calling
    {
        if (settingsChanged) // check for changes
        {
            SetGameSettings();
            settingsChanged = false;
        }
    }

    private void SetGameSettings()
    {
        Application.targetFrameRate = targetFrameRate;
        Camera.main.fieldOfView = targetFOV;
    }

}
