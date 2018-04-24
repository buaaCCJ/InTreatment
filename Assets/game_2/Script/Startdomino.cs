using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startdomino : MonoBehaviour {

    public Vector3 dir;
    public float force = 200;

    public void pushDonimo()
    {
        this.GetComponent<Rigidbody>().AddForce(dir * force);
    }    
}


