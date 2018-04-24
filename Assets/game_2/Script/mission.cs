using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission : MonoBehaviour {

    private DominoManger gameManager;
    private Light mhalo;
    private bool isComplete;

    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<DominoManger>();

        if (gameManager == null)
        {
            Debug.LogError("Startdomino not found!");
            enabled = false;
            return;
        }
    }

    void Start () {
        mhalo = transform.GetComponentInChildren<Light>();
        isComplete = false;

    }
	
	void Update () {
        if (transform.position.y < 0.73f && isComplete == false)
        {
            if (!gameManager.IsStartPush)
            {
                gameManager.LosetheGame();
            }
            CompleteMission();
        }
	}

    void CompleteMission()
    {
        if(isComplete == false)
        {
            gameManager.MissionCompleteNum++;
            isComplete = true;
            mhalo.enabled = false;
        }

    }
}
