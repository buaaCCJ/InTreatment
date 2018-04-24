using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 背景音乐脚本
/// </summary>
public class AudioBackground : MonoBehaviour {

    static AudioBackground StaticObject;

    private bool LastMusicOn = true;

    void Start()
    {

    }
    public static AudioBackground instance
    {
        get
        {
            if (StaticObject == null)
            {
                StaticObject = FindObjectOfType<AudioBackground>();
                DontDestroyOnLoad(StaticObject.gameObject);
            }
            return StaticObject;
        }
    }
    void Awake()
    {
        if (StaticObject == null)
        {
            StaticObject = this;
            DontDestroyOnLoad(this);
        }
        else if (this != StaticObject)
        {
            Destroy(gameObject);
        }

    }
    void Update()
    {
        if (MainController.musicOn != LastMusicOn)
        {
            LastMusicOn = MainController.musicOn;
            OnMusicChanged();
        }
    }
    private void OnMusicChanged()
    {
        if (MainController.musicOn)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            gameObject.GetComponent<AudioSource>().Pause();
        }
    }
}
