/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonScript : MonoBehaviour
{
    private AircraftControls aircraftControls = null;

    #region Selection Varibales
    public WeaponType selectedType = WeaponType.Empty;
    private WeaponType prevSelectedType = WeaponType.Empty;
    public bool mirrored = false; // do later
    public bool applyChanges = false;
    private bool listSelection = false;
    public bool moreThanOneStack = false;
    private int stackNumber = 1; // 1 - 2
    public List<string> innerTypeSelection = new List<string>();
    public int selectionInteger = 0;
    #endregion

    #region Weapon Variables
    // Gun
    public List<GameObject> gunPodList = new List<GameObject>();

    // Rockets
    public List<GameObject> smallRocketPodList = new List<GameObject>();
    public List<GameObject> mediumRocketPodList = new List<GameObject>();
    public List<GameObject> largeRocketPodList = new List<GameObject>();

    // AAM's
    public List<GameObject> IRList = new List<GameObject>();
    public List<GameObject> SARHList = new List<GameObject>();
    public List<GameObject> ARHList = new List<GameObject>();
    public List<GameObject> beamRiderList = new List<GameObject>();

    // Bombs
    public List<GameObject> smallBombList = new List<GameObject>();
    public List<GameObject> mediumBombList = new List<GameObject>();
    public List<GameObject> largeBombList = new List<GameObject>();
    public List<GameObject> GBUList = new List<GameObject>();
    public List<GameObject> CBUList = new List<GameObject>();
    public List<GameObject> napalmList = new List<GameObject>();

    // AGM
    public List<GameObject> AGMList = new List<GameObject>();
    public List<GameObject> ARMList = new List<GameObject>();
    public List<GameObject> AShMList = new List<GameObject>();

    // Drop Tank
    public List<GameObject> dropTankList = new List<GameObject>();

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (moreThanOneStack)
            stackNumber = 1;

        aircraftControls = transform.parent.GetComponent<AircraftControls>();

        selectedType = WeaponType.Empty; // for reseting type selection

        GetWeaponSelection();
    }

    // Update is called once per frame
    void Update()
    {

        if (!moreThanOneStack && stackNumber > 1)
            stackNumber = 1;

        if(prevSelectedType != selectedType)
        {
            listSelection = true;
        }

        if (listSelection)
        {
            innerTypeSelection.Clear();
            GetWeaponSelection();
            listSelection = false;
        }

        if (applyChanges)
        {
            if (transform.childCount > 0)
            {
                for(int i = 0; i < transform.childCount; i++)
                    Destroy(transform.GetChild(i));
            }

            ApplyChanges();
            aircraftControls.weaponsChanged = true;
            applyChanges = false;
        }

        prevSelectedType = selectedType;

    }

    /// <summary>
    /// Allows the selection of weapons
    /// </summary>
    private void GetWeaponSelection()
    {
        switch (selectedType)
        {
            case WeaponType.GunPod:
                for (int i = 0; i < gunPodList.Count; i++)
                    innerTypeSelection.Add(gunPodList[i].name);
                break;
            case WeaponType.IR:
                for (int i = 0; i < IRList.Count; i++)
                    innerTypeSelection.Add(IRList[i].name);
                break;
            case WeaponType.SARH:
                for (int i = 0; i < SARHList.Count; i++)
                    innerTypeSelection.Add(SARHList[i].name);
                break;
            case WeaponType.ARH:
                for (int i = 0; i < ARHList.Count; i++)
                    innerTypeSelection.Add(ARHList[i].name);
                break;
            case WeaponType.BeamRider:
                for (int i = 0; i < beamRiderList.Count; i++)
                    innerTypeSelection.Add(beamRiderList[i].name);
                break;
            case WeaponType.SmallBomb:
                for (int i = 0; i < smallBombList.Count; i++)
                    innerTypeSelection.Add(smallBombList[i].name);
                break;
            case WeaponType.MediumBomb:
                for (int i = 0; i < mediumBombList.Count; i++)
                    innerTypeSelection.Add(mediumBombList[i].name);
                break;
            case WeaponType.LargeBomb:
                for (int i = 0; i < largeBombList.Count; i++)
                    innerTypeSelection.Add(largeBombList[i].name);
                break;
            case WeaponType.GBU:
                for (int i = 0; i < GBUList.Count; i++)
                    innerTypeSelection.Add(GBUList[i].name);
                break;
            case WeaponType.CBU:
                for (int i = 0; i < CBUList.Count; i++)
                    innerTypeSelection.Add(CBUList[i].name);
                break;
            case WeaponType.Napalm:
                for (int i = 0; i < napalmList.Count; i++)
                    innerTypeSelection.Add(napalmList[i].name);
                break;
            case WeaponType.SmallRocket:
                for (int i = 0; i < smallRocketPodList.Count; i++)
                    innerTypeSelection.Add(smallRocketPodList[i].name);
                break;
            case WeaponType.MediumRocket:
                for (int i = 0; i < mediumRocketPodList.Count; i++)
                    innerTypeSelection.Add(mediumRocketPodList[i].name);
                break;
            case WeaponType.LargeRocket:
                for (int i = 0; i < largeRocketPodList.Count; i++)
                    innerTypeSelection.Add(largeRocketPodList[i].name);
                break;
            case WeaponType.AGM:
                for (int i = 0; i < AGMList.Count; i++)
                    innerTypeSelection.Add(AGMList[i].name);
                break;
            case WeaponType.ARM:
                for (int i = 0; i < ARMList.Count; i++)
                    innerTypeSelection.Add(ARMList[i].name);
                break;
            case WeaponType.AShM:
                for (int i = 0; i < AShMList.Count; i++)
                    innerTypeSelection.Add(AShMList[i].name);
                break;
            case WeaponType.ExternalFuelTank:
                for (int i = 0; i < dropTankList.Count; i++)
                    innerTypeSelection.Add(dropTankList[i].name);
                break;
            default: 
                break;
        }

    }

    /// <summary>
    /// Spawns the selected weapon onto the pylon
    /// </summary>
    private void ApplyChanges()
    {

        switch (selectedType)
        {
            case WeaponType.GunPod:
                Instantiate(gunPodList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.IR:
                Instantiate(IRList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.SARH:
                Instantiate(SARHList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.ARH:
                Instantiate(ARHList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.BeamRider:
                Instantiate(beamRiderList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.SmallBomb:
                Instantiate(smallBombList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.MediumBomb:
                Instantiate(mediumBombList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.LargeBomb:
                Instantiate(largeBombList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.GBU:
                Instantiate(GBUList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.CBU:
                Instantiate(CBUList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.Napalm:
                Instantiate(napalmList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.SmallRocket:
                Instantiate(smallRocketPodList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.MediumRocket:
                Instantiate(mediumRocketPodList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.LargeRocket:
                Instantiate(largeRocketPodList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.AGM:
                Instantiate(AGMList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.ARM:
                Instantiate(ARMList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.AShM:
                Instantiate(AShMList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            case WeaponType.ExternalFuelTank:
                Instantiate(dropTankList[selectionInteger], transform.position, Quaternion.identity, transform);
                break;
            default:
                break;
        }

    }

}
