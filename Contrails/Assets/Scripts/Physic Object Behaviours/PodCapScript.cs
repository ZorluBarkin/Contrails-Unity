using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodCapScript : MonoBehaviour
{

    private float deleteTime = 10f;
    private float deleteTimer = 0f;

    void FixedUpdate()
    {
        if(deleteTimer >= deleteTime)
            Destroy(gameObject);

        deleteTimer = Time.deltaTime;
    }
}
