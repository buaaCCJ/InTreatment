using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartMovie : MonoBehaviour {

    private bool isIn = false;
    private bool isYiqian = false;

    // Use this for initialization
    void Start()
    {
        Invoke("setInture", 0.1f);
        Invoke("setyiqianture", 1.2f);
    }

    private void setInture()
    {
        isIn = true;
    }

    private void setyiqianture()
    {
        isYiqian = true;
    }

    private void OnGUI()
    {
        if (isIn)
        {
            Handheld.PlayFullScreenMovie("logo_1.mp4", Color.black, FullScreenMovieControlMode.Hidden);
            isIn = false;
        }
        if (isYiqian)
        {
            Handheld.PlayFullScreenMovie("logo_2.mp4", Color.black, FullScreenMovieControlMode.Hidden);
            isYiqian = false;
            Invoke("turnscene",1.5f);
        }
    }

    void turnscene()
    {
        Application.LoadLevel(1);
    }
}
