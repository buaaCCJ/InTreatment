using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkGUIController : MonoBehaviour {

	void Start () {
        InvokeRepeating("CheckVisible", 0, 5);
	}
	
	void Update () {
	
	}

    void CheckVisible()
    {
        this.GetComponent<NetworkManagerHUD>().showGUI = MainController.NetworkTest;
    }
}
