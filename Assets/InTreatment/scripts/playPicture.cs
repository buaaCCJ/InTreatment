using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playPicture : MonoBehaviour {
    public float speed;
    public float startYPosition;
    public float StopYPosition;
    // Use this for initialization
    void Start()
    {
        
    transform.position= new Vector3(transform.position.x, startYPosition, transform.position.z);
    }
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
        if (transform.position.y >= StopYPosition)
        {
            transform.position = new Vector3(transform.position.x, StopYPosition, transform.position.z); ;
        }
	}
}
