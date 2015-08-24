using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level4Controller : MonoBehaviour {

	public Text winText;

	ArrayList healthyCells = new ArrayList();
	string state = "playing";

	// Use this for initialization
	void Start () {
		winText = winText.GetComponent<Text>();
		GameObject[] cells = GameObject.FindGameObjectsWithTag("cell");
		CellController currentCell;

		for (int i = 0; i < cells.Length; i++) {
			currentCell = cells[i].GetComponent<CellController>();

			if (currentCell.owner == "body") {
				healthyCells.Add(currentCell);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (state == "playing") {
			state = "won";

			for (int i = 0; i < healthyCells.Count; i++) {
				if (((CellController)healthyCells[i]).owner == "body") {
					state = "playing";
					break;
				}
			}
		}

		if (state == "won") {
			winText.enabled = true;

			if (Input.anyKey) {
				Application.LoadLevel("Level5");
			}
		}
	}
}
