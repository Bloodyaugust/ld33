using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level1Controller : MonoBehaviour {

	public GameObject healthyCell;
	public Text winText;

	CellController healthyCellController;

	// Use this for initialization
	void Start () {
		healthyCellController = healthyCell.GetComponent<CellController>();
		winText = winText.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		if (healthyCellController.owner == "player") {
			if (!winText.enabled) {
				winText.enabled = true;
			}

			if (Input.anyKey) {
				Application.LoadLevel("Level2");
			}
		}
	}
}
