using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType
{
    Gun,
    IR,
    SAHR,
    AHR,
    BeamRider,
    smallBomb,
    mediumBomb,
    largeBomb,
    GBU,
    CBU,
    Napalm,
    SmallRocket,
    MediumRocket,
    LargeRocket,
    AGM,
    ARM,
    AShM,
    Empty // only for use in pylon script
}

public class AircraftControls : MonoBehaviour
{
    #region Weaponry Variables
    public List<GameObject> pylonList = new List<GameObject>(); // pylons will always start at index 1, then comes the guns
    public List<GunScript> gunList = new List<GunScript>(); // comes after pylons untill end, currently
    public int pylonNumber = 0;
    private int pylonsAssigned = 0;

    // Rockets
    public List<RocketPodScript> smallRocketPodList = new List<RocketPodScript>();
    public List<RocketPodScript> mediumRocketPodList = new List<RocketPodScript>();
    public List<RocketPodScript> largeRocketPodList = new List<RocketPodScript>();

    // AAM's
    public List<AAMissileScript> IRList = new List<AAMissileScript>();
    public List<AAMissileScript> SAHRList = new List<AAMissileScript>();
    public List<AAMissileScript> AHRList = new List<AAMissileScript>();
    public List<AAMissileScript> beamRiderList = new List<AAMissileScript>();

    // Bombs
    public List<BombScript> smallBombList = new List<BombScript>();
    public List<BombScript> mediumBombList = new List<BombScript>();
    public List<BombScript> largeBombList = new List<BombScript>();
    public List<BombScript> GBUList = new List<BombScript>();
    public List<BombScript> CBUList = new List<BombScript>();
    public List<BombScript> napalmList = new List<BombScript>();

    // AGM
    public List<AGMScript> AGMList = new List<AGMScript>();
    public List<AGMScript> ARMList = new List<AGMScript>();
    public List<AGMScript> AShMList = new List<AGMScript>();
    #endregion
    
    public WeaponType currentSelection = WeaponType.Gun;
    //NOTE: a launching of a weapon is to delete its respective child model under the plane and intantiating it where the deleted object was.

    // Start is called before the first frame update
    void Start()
    {
        GetWeapons();
    }

    // gets input
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R)) // arm the selected weapons?
        {
            ArmWeapons();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            for(int i = 0; i < gunList.Count; i++)
                gunList[i].fire = true;
        }
        else
        {
            for (int i = 0; i < gunList.Count; i++)
            {
                if (gunList[i].fire == true)
                    gunList[i].fire = false;
            }
                
        }

    }

    /// <summary>
    /// Assigns the weapons under pylons to their respective list
    /// </summary>
    private void GetWeapons()
    {
        if (pylonList.Count == 0)
        {
            for (int i = 0; i < pylonNumber; i++)
                pylonList.Add(transform.GetChild(i + 1).gameObject);
        }

        if (gunList.Count == 0)
        {
            for (int i = 0; i < transform.childCount - pylonNumber - 1; i++)
                gunList.Add(transform.GetChild(i + pylonNumber + 1).gameObject.GetComponent<GunScript>());
        }

        if(smallRocketPodList.Count == 0) // need to reach to child and get the objects tag
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for(int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "SmallRocket")
                    {
                        smallRocketPodList.Add(pylonList[i].transform.GetChild(j).GetComponent<RocketPodScript>());
                    }
                }
            }
        }

        if(mediumRocketPodList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "MediumRocket")
                    {
                        smallRocketPodList.Add(pylonList[i].transform.GetChild(j).GetComponent<RocketPodScript>());
                    }
                }
            }
        }

        if(largeRocketPodList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "largeRocket")
                    {
                        smallRocketPodList.Add(pylonList[i].transform.GetChild(j).GetComponent<RocketPodScript>());
                    }
                }
            }
        }

        if(IRList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "IR")
                    {
                        IRList.Add(pylonList[i].transform.GetChild(j).GetComponent<AAMissileScript>());
                    }
                }
            }
        }

        if(SAHRList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "SAHR")
                    {
                        SAHRList.Add(pylonList[i].transform.GetChild(j).GetComponent<AAMissileScript>());
                    }
                }
            }
        }

        if(AHRList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "AHR")
                    {
                        AHRList.Add(pylonList[i].transform.GetChild(j).GetComponent<AAMissileScript>());
                    }
                }
            }
        }

        if(smallBombList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "SmallBomb")
                    {
                        smallBombList.Add(pylonList[i].transform.GetChild(j).GetComponent<BombScript>());
                    }
                }
            }
        }

        if(mediumBombList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "MediumBomb")
                    {
                        mediumBombList.Add(pylonList[i].transform.GetChild(j).GetComponent<BombScript>());
                    }
                }
            }
        }

        if(largeBombList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "LargeBomb")
                    {
                        largeBombList.Add(pylonList[i].transform.GetChild(j).GetComponent<BombScript>());
                    }
                }
            }
        }
        
        if(GBUList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "GBU")
                    {
                        GBUList.Add(pylonList[i].transform.GetChild(j).GetComponent<BombScript>());
                    }
                }
            }
        }
        
        if(beamRiderList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "BeamRider")
                    {
                        beamRiderList.Add(pylonList[i].transform.GetChild(j).GetComponent<AAMissileScript>());
                    }
                }
            }
        }
         
        if(CBUList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "CBU")
                    {
                        CBUList.Add(pylonList[i].transform.GetChild(j).GetComponent<BombScript>());
                    }
                }
            }
        }
         
        if(napalmList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "Napalm")
                    {
                        napalmList.Add(pylonList[i].transform.GetChild(j).GetComponent<BombScript>());
                    }
                }
            }
        }
         
        if(AGMList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "AGM")
                    {
                        AGMList.Add(pylonList[i].transform.GetChild(j).GetComponent<AGMScript>());
                    }
                }
            }
        }
         
        if(ARMList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "ARM")
                    {
                        ARMList.Add(pylonList[i].transform.GetChild(j).GetComponent<AGMScript>());
                    }
                }
            }
        }
         
        if(AShMList.Count == 0)
        {
            for (int i = 0; i < pylonList.Count; i++)
            {
                for (int j = 0; j < pylonList[i].transform.childCount; j++)
                {
                    if (pylonList[i].transform.GetChild(j).tag == "AShM")
                    {
                        AShMList.Add(pylonList[i].transform.GetChild(j).GetComponent<AGMScript>());
                    }
                }
            }
        }

    }

    /// <summary>
    /// Arms weapons which are in lists of their respective types, only arms the selected type
    /// </summary>
    private void ArmWeapons()
    {
        if (currentSelection == WeaponType.Gun)
        {
            for (int i = 0; i < gunList.Count; i++)
            {
                if (gunList[i].armed == false)
                    gunList[i].armed = true;
                else
                    gunList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.IR)
        {
            for (int i = 0; i < IRList.Count; i++)
            {
                if (IRList[i].armed == false)
                    IRList[i].armed = true;
                else
                    IRList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.SAHR)
        {
            for (int i = 0; i < SAHRList.Count; i++)
            {
                if (SAHRList[i].armed == false)
                    SAHRList[i].armed = true;
                else
                    SAHRList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.AHR)
        {
            for (int i = 0; i < AHRList.Count; i++)
            {
                if (AHRList[i].armed == false)
                    AHRList[i].armed = true;
                else
                    AHRList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.smallBomb)
        {
            for (int i = 0; i < smallBombList.Count; i++)
            {
                if (smallBombList[i].armed == false)
                    smallBombList[i].armed = true;
                else
                    smallBombList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.mediumBomb)
        {
            for (int i = 0; i < mediumBombList.Count; i++)
            {
                if (mediumBombList[i].armed == false)
                    mediumBombList[i].armed = true;
                else
                    mediumBombList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.largeBomb)
        {
            for (int i = 0; i < largeBombList.Count; i++)
            {
                if (largeBombList[i].armed == false)
                    largeBombList[i].armed = true;
                else
                    largeBombList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.GBU)
        {
            for (int i = 0; i < GBUList.Count; i++)
            {
                if (GBUList[i].armed == false)
                    GBUList[i].armed = true;
                else
                    GBUList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.SmallRocket)
        {
            for (int i = 0; i < smallRocketPodList.Count; i++)
            {
                if (smallRocketPodList[i].armed == false)
                    smallRocketPodList[i].armed = true;
                else
                    smallRocketPodList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.MediumRocket)
        {
            for (int i = 0; i < mediumRocketPodList.Count; i++)
            {
                if (mediumRocketPodList[i].armed == false)
                    mediumRocketPodList[i].armed = true;
                else
                    mediumRocketPodList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.LargeRocket)
        {
            for (int i = 0; i < largeRocketPodList.Count; i++)
            {
                if (largeRocketPodList[i].armed == false)
                    largeRocketPodList[i].armed = true;
                else
                    largeRocketPodList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.BeamRider)
        {
            for (int i = 0; i < beamRiderList.Count; i++)
            {
                if (beamRiderList[i].armed == false)
                    beamRiderList[i].armed = true;
                else
                    beamRiderList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.CBU)
        {
            for (int i = 0; i < CBUList.Count; i++)
            {
                if (CBUList[i].armed == false)
                    CBUList[i].armed = true;
                else
                    CBUList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.Napalm)
        {
            for (int i = 0; i < napalmList.Count; i++)
            {
                if (napalmList[i].armed == false)
                    napalmList[i].armed = true;
                else
                    napalmList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.AGM)
        {
            for (int i = 0; i < AGMList.Count; i++)
            {
                if (AGMList[i].armed == false)
                    AGMList[i].armed = true;
                else
                    AGMList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.ARM)
        {
            for (int i = 0; i < AGMList.Count; i++)
            {
                if (AGMList[i].armed == false)
                    AGMList[i].armed = true;
                else
                    AGMList[i].armed = false;
            }
        }
        else if (currentSelection == WeaponType.AShM)
        {
            for (int i = 0; i < AGMList.Count; i++)
            {
                if (AGMList[i].armed == false)
                    AGMList[i].armed = true;
                else
                    AGMList[i].armed = false;
            }
        }

    }
}
