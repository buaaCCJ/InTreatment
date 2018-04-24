using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 音效管理器
/// </summary>
public class VoiceManager : MonoBehaviour{

    public AudioSource MouseEnterAudio;

	void Start () {
        foreach (Button item in Resources.FindObjectsOfTypeAll(typeof(Button)))
        {
           item.onClick.AddListener(() => { OnMouseEnter(); });
        }       
    }

    void OnMouseEnter()
    {
        if (MainController.soundOn)
        {
            MouseEnterAudio.Play();
        }
    }
}
