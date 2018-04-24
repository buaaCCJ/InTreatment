using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class gamemanager : MonoBehaviour {
    public Flowchart fl;
	// Use this for initialization
	void Start () {
        fl.SendFungusMessage("start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
