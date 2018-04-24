using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScrip1t : MonoBehaviour {
    private SpriteRenderer r;
    private float x = 0.0f;
    // Use this for initialization
    void Start () {
        r = gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (x < 1.0)
        {
            x += 0.005f;
        }

        r.color = new Color(0f, 0f, 0f, x);
    }
}
