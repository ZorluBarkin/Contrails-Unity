/*  
 * Copyright December 2022 Barkın Zorlu 
 * All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum WeaponType
{
    Empty,
    GunPod,
    IR,
    SARH,
    ARH,
    BeamRider,
    SmallBomb,
    MediumBomb,
    LargeBomb,
    GBU,
    CBU,
    Napalm,
    SmallRocket,
    MediumRocket,
    LargeRocket,
    AGM,
    ARM,
    AShM,
    ExternalFuelTank
    // only for use in pylon script
}

public class AircraftControls : MonoBehaviour
{
    #region Weaponry Variables
    [Header("Weaponry Variables")]
    public List<GameObject> pylonList = new List<GameObject>(); // pylons will always start at index 1, then comes the guns
    public int pylonNumber = 0;

    // Rockets
    public List<RocketPodScript> smallRocketPodList = new List<RocketPodScript>();
    public List<RocketPodScript> mediumRocketPodList = new List<RocketPodScript>();
    public List<RocketPodScript> largeRocketPodList = new List<RocketPodScript>();

    // AAM's
    public List<AAMissileScript> IRList = new List<AAMissileScript>();
    public List<AAMissileScript> SARHList = new List<AAMissileScript>();
    public List<AAMissileScript> ARHList = new List<AAMissileScript>();
    public List<AAMissileScript> beamRiderList = new List<AAMissileScript>();

    // Bombs
    public List<BombScript> smallBombList = new List<BombScript>();
    public List<BombScript> mediumBombList = new List<BombScript>();
    public List<BombScript> largeBombList = new List<BombScript>();
    public List<BombScript> GBUList = new List<BombScript>();
    public List<BombScript> CBUList = new List<BombScript>();
    public List<NapalmScript> napalmList = new List<NapalmScript>();

    // AGM
    public List<AGMScript> AGMList = new List<AGMScript>();
    public List<AGMScript> ARMList = new List<AGMScript>();
    public List<AGMScript> AShMList = new List<AGMScript>();

    // GUNS
    public List<GunScript> gunList = new List<GunScript>();
    public List<GunPodScript> gunPodList = new List<GunPodScript>();
    public int totalAmmo = 0;

    // Drop Tank
    public List<DropTankScript> dropTankList = new List<DropTankScript>();

    // TODO: Add ripple fire bool, right here with a fixed interval then add it to UseWeapon method
    public int dropSeries = 1; // the amount of ordinance to drop at one time.
    public static string _weaponSelectionString = null; // TODO: change this to non static
    #endregion

    public WeaponType currentSelection = WeaponType.Empty;
    private int weaponTypeCount = 0;
    //NOTE: a launching of a weapon is to delete its respective child model under the plane and intantiating it where the deleted object was.
    public bool weaponsChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        GetWeapons();

        weaponTypeCount = Enum.GetNames(typeof(WeaponType)).Length - 1;
        currentSelection = WeaponType.Empty;

        //if (flyPoint) // does not work
        //    flyPoint.transform.position = Camera.main.GetComponentInChildren<Transform>().position; // fly point should always be 1st child of main cam
    }

    // gets input
    void Update()
    {

        if (weaponsChanged)
        {
            ClearWeaponLists();
            GetWeapons();
            weaponsChanged = false;
        }

        GetWeaponInput();

    }

    private void LateUpdate()
    {
        totalAmmo = CalculateAmmo();
    }

    private int CalculateAmmo()
    {
        int ammo = 0;
        for (int i = 0; i < gunList.Count; i++)
            ammo += gunList[i].ammoCount;

        for (int i = 0; i < gunPodList.Count; i++)
            ammo += gunPodList[i].totalAmmo;

        return ammo;
    }

    /// <summary>
    /// Assigns the weapons under pylons to their respective list
    /// </summary>
    private void GetWeapons()
    {
        if (pylonList.Count == 0) // gets the pylons
        {
            for (int i = 0; i < pylonNumber; i++)
                pylonList.Add(transform.GetChild(i + 1).gameObject);
        }

        if (gunList.Count == 0) // gets the guns
        {
            for (int i = 0; i < transform.childCount - pylonNumber - 1; i++)
                gunList.Add(transform.GetChild(i + pylonNumber + 1).gameObject.GetComponent<GunScript>());
        }

        for (int i = 0; i < pylonList.Count; i++)
        {

            for (int j = 0; j < pylonList[i].transform.childCount; j++)
            {
                WeaponTypeScript weaponTypeScript = pylonList[i].transform.GetChild(j).GetComponent<WeaponTypeScript>();
                GameObject weapon = pylonList[i].transform.GetChild(j).gameObject;

                switch (weaponTypeScript.weaponType)
                {
                    case WeaponType.GunPod:
                        gunPodList.Add(weapon.GetComponent<GunPodScript>());
                        break;
                    case WeaponType.IR:
                        IRList.Add(weapon.GetComponent<AAMissileScript>());
                        break;
                    case WeaponType.SARH:
                        SARHList.Add(weapon.GetComponent<AAMissileScript>());
                        break;
                    case WeaponType.ARH:
                        ARHList.Add(weapon.GetComponent<AAMissileScript>());
                        break;
                    case WeaponType.BeamRider:
                        beamRiderList.Add(weapon.GetComponent<AAMissileScript>());
                        break;
                    case WeaponType.SmallBomb:
                        smallBombList.Add(weapon.GetComponent<BombScript>());
                        break;
                    case WeaponType.MediumBomb:
                        mediumBombList.Add(weapon.GetComponent<BombScript>());
                        break;
                    case WeaponType.LargeBomb:
                        largeBombList.Add(weapon.GetComponent<BombScript>());
                        break;
                    case WeaponType.GBU:
                        GBUList.Add(weapon.GetComponent<BombScript>());
                        break;
                    case WeaponType.CBU:
                        CBUList.Add(weapon.GetComponent<BombScript>());
                        break;
                    case WeaponType.Napalm:
                        napalmList.Add(weapon.GetComponent<NapalmScript>());
                        break;
                    case WeaponType.SmallRocket:
                        smallRocketPodList.Add(weapon.GetComponent<RocketPodScript>());
                        break;
                    case WeaponType.MediumRocket:
                        mediumRocketPodList.Add(weapon.GetComponent<RocketPodScript>());
                        break;
                    case WeaponType.LargeRocket:
                        largeRocketPodList.Add(weapon.GetComponent<RocketPodScript>());
                        break;
                    case WeaponType.AGM:
                        AGMList.Add(weapon.GetComponent<AGMScript>());
                        break;
                    case WeaponType.ARM:
                        ARMList.Add(weapon.GetComponent<AGMScript>());
                        break;
                    case WeaponType.AShM:
                        AShMList.Add(weapon.GetComponent<AGMScript>());
                        break;
                    case WeaponType.ExternalFuelTank:
                        dropTankList.Add(weapon.GetComponent<DropTankScript>());
                        break;
                    default: // Do nothing
                        break;

                }

            }

        }

    }

    /// <summary>
    /// Clears every List except gunlist (guns are child of aircraft)
    /// </summary>
    private void ClearWeaponLists()
    {
        // Rockets
        smallRocketPodList.Clear();
        mediumRocketPodList.Clear();
        largeRocketPodList.Clear();

        // AAMs
        IRList.Clear();
        SARHList.Clear();
        ARHList.Clear();
        beamRiderList.Clear();

        // Bombs
        smallBombList.Clear();
        mediumBombList.Clear();
        largeBombList.Clear();
        GBUList.Clear();
        CBUList.Clear();
        napalmList.Clear();

        // AGMs
        AGMList.Clear();
        ARMList.Clear();
        AShMList.Clear();

        // Gun Pods
        gunPodList.Clear();

        // Drpo Tanks
        dropTankList.Clear();
    }

    /// <summary>
    /// Arms weapons which are in lists of their respective types, only arms the selected type
    /// </summary>
    private void ArmWeapons()
    {
        switch (currentSelection)
        {
            case WeaponType.GunPod:
                for (int i = 0; i < gunList.Count; i++)
                {
                    if (!gunList[i].armed)
                        gunList[i].armed = true;
                    else
                        gunList[i].armed = false;
                }

                for (int i = 0; i < gunPodList.Count; i++)
                {
                    if (!gunPodList[i].armed)
                        gunPodList[i].armed = true;
                    else
                        gunPodList[i].armed = false;
                }
                break;
            case WeaponType.IR:
                for (int i = 0; i < IRList.Count; i++)
                {
                    if (!IRList[i].armed)
                        IRList[i].armed = true;
                    else
                        IRList[i].armed = false;
                }
                break;
            case WeaponType.SARH:
                for (int i = 0; i < SARHList.Count; i++)
                {
                    if (!SARHList[i].armed)
                        SARHList[i].armed = true;
                    else
                        SARHList[i].armed = false;
                }
                break;
            case WeaponType.ARH:
                for (int i = 0; i < ARHList.Count; i++)
                {
                    if (!ARHList[i].armed)
                        ARHList[i].armed = true;
                    else
                        ARHList[i].armed = false;
                }
                break;
            case WeaponType.BeamRider:
                for (int i = 0; i < beamRiderList.Count; i++)
                {
                    if (!beamRiderList[i].armed)
                        beamRiderList[i].armed = true;
                    else
                        beamRiderList[i].armed = false;
                }
                break;
            case WeaponType.SmallBomb:
                for (int i = 0; i < smallBombList.Count; i++)
                {
                    if (!smallBombList[i].armed)
                        smallBombList[i].armed = true;
                    else
                        smallBombList[i].armed = false;
                }
                break;
            case WeaponType.MediumBomb:
                for (int i = 0; i < mediumBombList.Count; i++)
                {
                    if (!mediumBombList[i].armed)
                        mediumBombList[i].armed = true;
                    else
                        mediumBombList[i].armed = false;
                }
                break;
            case WeaponType.LargeBomb:
                for (int i = 0; i < largeBombList.Count; i++)
                {
                    if (!largeBombList[i].armed)
                        largeBombList[i].armed = true;
                    else
                        largeBombList[i].armed = false;
                }
                break;
            case WeaponType.GBU:
                for (int i = 0; i < GBUList.Count; i++)
                {
                    if (!GBUList[i].armed)
                        GBUList[i].armed = true;
                    else
                        GBUList[i].armed = false;
                }
                break;
            case WeaponType.CBU:
                for (int i = 0; i < CBUList.Count; i++)
                {
                    if (!CBUList[i].armed)
                        CBUList[i].armed = true;
                    else
                        CBUList[i].armed = false;
                }
                break;
            case WeaponType.Napalm:
                for (int i = 0; i < napalmList.Count; i++)
                {
                    if (!napalmList[i].armed)
                        napalmList[i].armed = true;
                    else
                        napalmList[i].armed = false;
                }
                break;
            case WeaponType.SmallRocket:
                for (int i = 0; i < smallRocketPodList.Count; i++)
                {
                    if (!smallRocketPodList[i].armed)
                        smallRocketPodList[i].armed = true;
                    else
                        smallRocketPodList[i].armed = false;
                }
                break;
            case WeaponType.MediumRocket:
                for (int i = 0; i < mediumRocketPodList.Count; i++)
                {
                    if (!mediumRocketPodList[i].armed)
                        mediumRocketPodList[i].armed = true;
                    else
                        mediumRocketPodList[i].armed = false;
                }
                break;
            case WeaponType.LargeRocket:
                for (int i = 0; i < largeRocketPodList.Count; i++)
                {
                    if (!largeRocketPodList[i].armed)
                        largeRocketPodList[i].armed = true;
                    else
                        largeRocketPodList[i].armed = false;
                }
                break;
            case WeaponType.AGM:
                for (int i = 0; i < AGMList.Count; i++)
                {
                    if (!AGMList[i].armed)
                        AGMList[i].armed = true;
                    else
                        AGMList[i].armed = false;
                }
                break;
            case WeaponType.ARM:
                for (int i = 0; i < AGMList.Count; i++)
                {
                    if (!AGMList[i].armed)
                        AGMList[i].armed = true;
                    else
                        AGMList[i].armed = false;
                }
                break;
            case WeaponType.AShM:
                for (int i = 0; i < AGMList.Count; i++)
                {
                    if (!AGMList[i].armed)
                        AGMList[i].armed = true;
                    else
                        AGMList[i].armed = false;
                }
                break;
            default: // do nothing
                break;
        }

    }

    private void GetWeaponInput()
    { // TODO: need to make these controls remappable to any key the user wants
        #region Weapon Inputs
        // TODO: This goes to -1 fix it
        if (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSelection++;
            _weaponSelectionString = "Weapon Selected: " + currentSelection.ToString();

            if ((int)currentSelection >= weaponTypeCount)
                currentSelection = 0;
        }
        else if (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSelection--;
            _weaponSelectionString = "Weapon Selected: " + currentSelection.ToString();

            if ((int)currentSelection < 0)
                currentSelection = (WeaponType)weaponTypeCount;
        }

        if (Input.GetKeyDown("[+]"))
        {
            dropSeries++;
        }
        else if (Input.GetKeyDown("[-]"))
        {
            dropSeries--;

            if (dropSeries < 1)
                dropSeries = 1;
        }

        if (Input.GetKeyDown(KeyCode.R)) // arm the selected weapons?
        {
            ArmWeapons();
        }

        if (currentSelection == WeaponType.GunPod || currentSelection == WeaponType.SmallRocket || 
            currentSelection == WeaponType.MediumRocket || currentSelection == WeaponType.LargeRocket)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                UseWeapon();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                CeaseFire();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                UseWeapon();
            }
        }


        #endregion

        // lock mouse inputs, used for looking around // currently implemented in HUD.cs may move it here
        //if (Input.GetKey(KeyCode.C))
        //{
            
        //}

        // ittarate over flap settings
        if (Input.GetKey(KeyCode.F))
        {

        }

        // gears up or down
        if (Input.GetKey(KeyCode.G))
        {

        }
    }

    /// <summary>
    /// Lanuches / Fires the selected weapon
    /// </summary>
    private void UseWeapon()
    {
        switch (currentSelection)
        {
            case WeaponType.GunPod:
                for (int i = 0; i < gunList.Count; i++)
                    gunList[i].fire = true;

                for (int i = 0; i < gunPodList.Count; i++)
                    gunPodList[i].fire = true;
                break;
            case WeaponType.IR:
                //if(IRList.Count > 0)
                //{
                //    IRList[0].active = true;
                //    IRList.RemoveAt(0);
                //}
                break;
            case WeaponType.SARH:
                //for (int i = 0; i < SARHList.Count; i++)
                //{
                //    if (!SARHList[i].armed)
                //        SARHList[i].armed = true;
                //    else
                //        SARHList[i].armed = false;
                //}
                break;
            case WeaponType.ARH:
                //for (int i = 0; i < ARHList.Count; i++)
                //{
                //    if (!ARHList[i].armed)
                //        ARHList[i].armed = true;
                //    else
                //        ARHList[i].armed = false;
                //}
                break;
            case WeaponType.BeamRider:
                //for (int i = 0; i < beamRiderList.Count; i++)
                //{
                //    if (!beamRiderList[i].armed)
                //        beamRiderList[i].armed = true;
                //    else
                //        beamRiderList[i].armed = false;
                //}
                break;
            case WeaponType.SmallBomb:
                if (smallBombList.Count > dropSeries)
                {
                    for (int i = 0; i < dropSeries; i++)
                    {
                        if (!smallBombList[i].armed)
                            return;
                        smallBombList[i].launch = true;
                        smallBombList.RemoveAt(i);
                    }
                }
                else
                {
                    for (int i = 0; i < smallBombList.Count; i++)
                    {
                        if (!smallBombList[i].armed)
                            return;
                        smallBombList[i].launch = true;
                        smallBombList.RemoveRange(0, smallBombList.Count);
                    }
                        
                }
                break;
            case WeaponType.MediumBomb:
                if (mediumBombList.Count > dropSeries)
                {
                    for (int i = 0; i < dropSeries; i++)
                    {
                        if (!mediumBombList[i].armed)
                            return;
                        mediumBombList[i].launch = true;
                        mediumBombList.RemoveAt(i);
                        //mediumBombList[i] = null;
                    }
                }
                else
                {
                    for (int i = 0; i < mediumBombList.Count; i++)
                    {
                        if (!mediumBombList[i].armed)
                            return;
                        mediumBombList[i].launch = true;
                        mediumBombList.RemoveRange(0, mediumBombList.Count);
                    }
                        
                }
                break;
            case WeaponType.LargeBomb:
                if (largeBombList.Count > dropSeries)
                {
                    for (int i = 0; i < dropSeries; i++)
                    {
                        if (!largeBombList[i].armed)
                            return;
                        largeBombList[i].launch = true;
                        largeBombList.RemoveAt(i);
                    }
                        
                }
                else
                {
                    for (int i = 0; i < largeBombList.Count; i++)
                    {
                        if (!largeBombList[i].armed)
                            return;
                        largeBombList[i].launch = true;
                        largeBombList.RemoveRange(0, largeBombList.Count);
                    }
                        
                }
                break;
            case WeaponType.GBU:
                if(GBUList.Count > 0)
                {
                    if (!GBUList[0].armed)
                        return;
                    GBUList[0].launch = true;
                    GBUList.RemoveAt(0);
                }
                break;
            case WeaponType.CBU:
                if (CBUList.Count > dropSeries)
                {
                    for (int i = 0; i < dropSeries; i++)
                    {
                        CBUList[i].launch = true;
                        CBUList.RemoveAt(i);
                    }
                }
                else
                {
                    for (int i = 0; i < CBUList.Count; i++)
                    {
                        CBUList[i].launch = true;
                        CBUList.RemoveRange(0, CBUList.Count);
                    }
                        
                }
                break;
            case WeaponType.Napalm:
                if (napalmList.Count > dropSeries) // TODO: This breaks with drop series more than one
                {
                    for (int i = 0; i < dropSeries; i++)
                    {
                        if (!napalmList[i].armed)
                            return;
                        napalmList[i].launch = true;
                        napalmList.RemoveAt(i);
                    }
                }
                else
                {
                    for (int i = 0; i < napalmList.Count; i++)
                    {
                        if (!napalmList[i].armed)
                            return;
                        napalmList[i].launch = true;
                        napalmList.RemoveRange(0, napalmList.Count);
                    }
                        
                }
                break;
            case WeaponType.SmallRocket:
                for(int i = 0; i < smallRocketPodList.Count; i++)
                    smallRocketPodList[i].launch = true;
                break;
            case WeaponType.MediumRocket:
                for (int i = 0; i < mediumRocketPodList.Count; i++)
                    mediumRocketPodList[i].launch = true;
                break;
            case WeaponType.LargeRocket:
                for (int i = 0; i < largeRocketPodList.Count; i++)
                    largeRocketPodList[i].launch = true;
                break;
            case WeaponType.AGM:
                //for (int i = 0; i < AGMList.Count; i++)
                //{
                //    if (!AGMList[i].armed)
                //        AGMList[i].armed = true;
                //    else
                //        AGMList[i].armed = false;
                //}
                break;
            case WeaponType.ARM:
                //for (int i = 0; i < AGMList.Count; i++)
                //{
                //    if (!AGMList[i].armed)
                //        AGMList[i].armed = true;
                //    else
                //        AGMList[i].armed = false;
                //}
                break;
            case WeaponType.AShM:
                //for (int i = 0; i < AGMList.Count; i++)
                //{
                //    if (!AGMList[i].armed)
                //        AGMList[i].armed = true;
                //    else
                //        AGMList[i].armed = false;
                //}
                break;
            default: // do nothing
                break;
        }
    }

    /// <summary>
    /// Ceases the firing of selected weapon, guns rocket pods etc.
    /// </summary>
    private void CeaseFire()
    {
        switch (currentSelection)
        {
            case WeaponType.GunPod:
                for (int i = 0; i < gunList.Count; i++)
                    gunList[i].fire = false;

                for (int i = 0; i < gunPodList.Count; i++)
                    gunPodList[i].fire = false;
                break;
            case WeaponType.SmallRocket:
                for (int i = 0; i < smallRocketPodList.Count; i++)
                    smallRocketPodList[i].launch = false;
                break;
            case WeaponType.MediumRocket:
                for (int i = 0; i < mediumRocketPodList.Count; i++)
                    mediumRocketPodList[i].launch = false;
                break;
            case WeaponType.LargeRocket:
                for (int i = 0; i < largeRocketPodList.Count; i++)
                    largeRocketPodList[i].launch = false;
                break;
            default: // do nothing
                break;
        }
    }

    private void FlyToPoint(Transform flyPoint)
    {

    }
}
