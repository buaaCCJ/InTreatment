using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoHome : MonoBehaviour {

    private DominoManger gameManager;

    private int currentdnum;
    private MeshRenderer[] md;

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

    void Start () {
        currentdnum = 3;
        md = new MeshRenderer[3];
        for(int i=0;i< currentdnum;i++)
        {
            md[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }
    }
	
	void Update () {
        if (currentdnum == 0)
        {
            currentdnum = 3;
            Invoke("getthreenew",0.5f);
        }
	}

    void getthreenew()
    {
        for (int i = 0; i < currentdnum; i++)
        {
            md[i].enabled = true;
        }
    }

    public void RemoveOne()
    {
        transform.GetChild(currentdnum-1).GetComponent<MeshRenderer>().enabled = false;
        currentdnum --;
        gameManager.BuildingDonimo("Domino");
    }
}
