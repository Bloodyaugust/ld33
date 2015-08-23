using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject selection;
	public GameObject target;

	CellController selectedCell;
	SpriteRenderer selectionRenderer;
	SpriteRenderer targetRenderer;

	// Use this for initialization
	void Start () {
		selectionRenderer = selection.GetComponent<SpriteRenderer>();
		targetRenderer = target.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		CellController targetCell;
		float selectScale, targetScale;

		if (selectedCell) {
			selectScale = (Mathf.Cos(Time.time * 4) * 0.25f) + (3f * selectedCell.transform.localScale.x);

			selection.transform.localScale = new Vector3(selectScale, selectScale, 1);

			targetCell = selectedCell.GetTargetCell();
			if (targetCell) {
				if (Input.GetAxis("Detarget") > 0) {
					selectedCell.Detarget();
					DisableTarget();
				} else {
					targetScale = (Mathf.Cos(Time.time * 4) * 0.25f) + (3f * targetCell.transform.localScale.x);

					target.transform.localScale = new Vector3(targetScale, targetScale, 1);
				}
			} else {
				DisableTarget();
			}
		}
	}

	void EnableSelection(Vector3 target) {
		CellController targetCell = selectedCell.GetTargetCell();

		selectionRenderer.enabled = true;
		selectionRenderer.transform.position = target;

		if (targetCell) {
			targetRenderer.enabled = true;
			targetRenderer.transform.position = targetCell.transform.position;
		} else {
			targetRenderer.enabled = false;
		}
	}

	void EnableTarget(Vector3 target) {
		targetRenderer.enabled = true;
		targetRenderer.transform.position = target;
	}

	void DisableSelection() {
		selectionRenderer.enabled = false;
		targetRenderer.enabled = false;
	}

	void DisableTarget() {
		targetRenderer.enabled = false;
	}

	public void SelectCell(CellController cell) {
		selectedCell = cell;
		EnableSelection(cell.transform.position);
	}

	public void TargetCell(CellController cell) {
		selectedCell.Target(cell);
		EnableTarget(cell.transform.position);
	}
}
