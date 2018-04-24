using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoAudioManger : MonoBehaviour {

    public AudioSource d_up;
    public AudioSource d_down;
    public AudioSource d_knock;
    public AudioSource d_delete;
    public AudioSource d_startpush;
    public AudioSource d_restart;


    private static DominoAudioManger instance;
    public static DominoAudioManger Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }
    void Start () {
		
	}
	
	void Update () {
		
	}
}
