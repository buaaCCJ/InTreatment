using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dir_Controller : MonoBehaviour {

    DominoManger gameManager;

    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<DominoManger>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
            enabled = false;
            return;
        }
    }

    void Start () {
        transform.localScale = Vector3.one * 0.35f;
        transform.position = gameManager.mStarter.transform.position + Vector3.up;
    }
	
	void Update () {

        if (gameManager.IsStartPush)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = Vector3.one * 0.35f;
            transform.position = gameManager.mStarter.transform.position + Vector3.up*1.5f +
                (Vector3.forward * Mathf.Sin(4 * Time.time) * 0.2f);

        }
	}
}
