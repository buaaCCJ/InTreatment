using UnityEngine;
using System.Collections;
using Fungus;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
    public Flowchart flowchart;
    
	public Puzzle[] Puzzles;
	int puzzleNumber = 9;
	int j = 0;
	int k = 0;
	Vector3[] PosNine = new Vector3[] {
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
        Vector3.zero
    };
	int[] Init = { 0, 0, 0, 0, 0, 0, 0, 0 ,0 };
	int[] InitRight = { 0, 0, 0, 0, 0, 0, 0, 0 ,0 };


	// Use this for initialization
	void Start () {
		for (int i = 0; i < puzzleNumber; i++) {
			PosNine [i] = Puzzles [i].transform.position;
			Puzzles [i].posPuz [0] = j;
			Puzzles [i].posPuz [1] = k++;
			if (k > 2) {
				j++;
				k = 0;
			}
		}
		for (int i = 0; i < puzzleNumber; i++) {
			Init [i] = i;
			InitRight [i] = i;
		}
		ShuffleRight(InitRight);
		for (int i = 0; i < puzzleNumber; i++) {
			Puzzles [i].posPuz [0] = InitRight [i] / 3;
			Puzzles [i].posPuz [1] = InitRight [i] % 3;
			Puzzles [i].transform.position = PosNine [InitRight [i]];
		}
	}
	
	void Update () {
        if (Success())
        {
            for (int i = 0; i < puzzleNumber; i++)
            {
                Puzzles[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            flowchart.SendFungusMessage("over");
        }
	}

	void Shuffle(int[] intArray)
	{
		for (int i = 0; i < intArray.Length - 1; i++)
		{
			int temp = intArray[i];
			int randomIndex = Random.Range(0, intArray.Length - 1);
			intArray[i] = intArray[randomIndex];
			intArray[randomIndex] = temp;
		}
	}

	void ShuffleRight(int[] intArray){
		for (int i = 0; i < 10; i++) {
			Shuffle (Init);
			int temp = InitRight [Init[0]];
			InitRight [Init[0]] = InitRight [Init[1]];
			InitRight [Init[1]] = InitRight [Init[2]];
			InitRight [Init[2]] = temp;
		}
	}

	bool Success(){
		for (int i = 0; i < puzzleNumber; i++) {
			int posNow = Puzzles [i].posPuz [0] * 3 + Puzzles [i].posPuz [1];
			if (Puzzles [i].numPuz != posNow)
				return false;
		}

		return true;
	}
    
}
