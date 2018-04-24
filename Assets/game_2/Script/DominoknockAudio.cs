using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoknockAudio : MonoBehaviour
{

    private DominoAudioManger mAudioM;

    void Awake()
    {
        mAudioM = DominoAudioManger.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
            mAudioM.d_knock.Play();
    }
}
