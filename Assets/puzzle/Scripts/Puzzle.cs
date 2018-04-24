using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour {

	public Puzzle Blank;
	public int numPuz;
	public int[] posPuz={0,0};
    public AudioSource movepuzzleAudio;

    void OnMouseDown(){
		if (posPuz [0] == Blank.posPuz [0] && Mathf.Abs (posPuz [1] - Blank.posPuz [1]) == 1) {
			Exchange();
		}
		if (posPuz [1] == Blank.posPuz [1] && Mathf.Abs (posPuz [0] - Blank.posPuz [0]) == 1) {
			Exchange();
		}
	}

	void Exchange(){
		Vector3 temp = transform.position;
		transform.position = Blank.transform.position;
		Blank.transform.position = temp;
		int[] tem = posPuz;
		posPuz = Blank.posPuz;
		Blank.posPuz = tem;
        movepuzzleAudio.Play();
    }
}
