using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
public class skip : MonoBehaviour {
    public Flowchart flowchart;
    private Button skipBtn;
    public AudioSource skipAudio;

	void Start () {
        skipBtn = transform.GetComponentInChildren<Button>();
        skipBtn.onClick.AddListener(onClick);      
	}
    void onClick()
    {
        skipAudio.Play();
        Debug.Log("click");
    }
	
}
