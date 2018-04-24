using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominosController : MonoBehaviour {

    private DominoManger gameManager;
    private Vector3 mPos;
    private Vector3 mEuler;
    private bool isComplete;

    void Awake()
    {
        gameManager = DominoManger.Instance;

        if (gameManager == null)
        {
            Debug.LogError("Startdomino not found!");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        isComplete = false;

    }

    void Update()
    {
        if (transform.position.y < 0.73f && isComplete == false)
        {
            if (!gameManager.IsStartPush)
            {
                gameManager.LosetheGame();
            }
        }
    }

    public void recondPosandEuler()
    {
        mPos = transform.position;
        mEuler = transform.eulerAngles;

    }

    public void getupagain()
    {
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.position = mPos;
        transform.eulerAngles = mEuler;
        transform.GetComponent<Rigidbody>().isKinematic = false;
    }

    
}
