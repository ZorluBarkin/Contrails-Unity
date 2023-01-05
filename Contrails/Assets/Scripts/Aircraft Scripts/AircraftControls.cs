using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftControls : MonoBehaviour
{
    private List<AAMissileScript> IRMissileArray = new List<AAMissileScript>();
    private int IRMissileCount = 0; 

    //NOTE: a launching of a weapon is to delete its respective child model under the plane and intantiating it where the deleted object was.

    // Start is called before the first frame update
    void Start()
    {
        if (IRMissileArray.Count == 0)
        {
            for (int i = 0; i < IRMissileCount; i++)
            {
                IRMissileArray.Add(new AAMissileScript());
            }
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown( KeyCode.Space) && IRMissileArray[0].targetLocked) // AAM Launch
        {

        }
    }
}
