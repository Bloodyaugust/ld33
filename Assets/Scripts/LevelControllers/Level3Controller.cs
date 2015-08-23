using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level3Controller : MonoBehaviour {

	public GameObject[] healthyCells;
	public Text winText;

	CellController[] healthyCellControllers;

	// Use this for initialization
	void Start () {
		healthyCellControllers = new CellController[healthyCells.Length];
		for (int i = 0; i < healthyCellControllers.Length; i++) {
			healthyCellControllers[i] = healthyCells[i].GetComponent<CellController>();
		}

		winText = winText.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		bool allCellsInfected = true;

		for (int i = 0; i < healthyCellControllers.Length; i++) {
			if (healthyCellControllers[i].owner == "body") {
				allCellsInfected = false;
				break;
			}
		}

		if (allCellsInfected) {
			winText.enabled = true;

			if (Input.anyKey) {
				Application.LoadLevel("Level4");
			}
		}
	}
}
