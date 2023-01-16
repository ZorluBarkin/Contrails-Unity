using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmScript : MonoBehaviour
{

    public Rigidbody rb = null;

    public float weight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Launched)
        //{
        //    transform.Rotate(5f, 0, 0); // 5 is good
        //}
            
    }
}
