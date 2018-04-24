using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignController : MonoBehaviour {

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

    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.position = Vector3.zero;

    }

    void Update()
    {
        if (gameManager.CurrentDomino == null)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = Vector3.one * 0.35f;
            transform.position = gameManager.CurrentDomino.transform.position + Vector3.up * (1.2f + Mathf.Sin(2 * Time.time) * 0.2f);
            transform.Rotate(new Vector3(120, 0, 0) * Time.deltaTime);
        }
    }
}
